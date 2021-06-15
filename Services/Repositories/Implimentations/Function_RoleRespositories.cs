﻿using AutoMapper;
using DLL;
using DLL.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class Function_RoleRespositories : IFunction_RoleRespositories
    {
        Datacontext db;
        IMapper mp;
        IRoleRespositories _IRoleRespositories;
        public Function_RoleRespositories(Datacontext datacontext, IMapper mapper, IRoleRespositories IRoleRespositories)
        {
            this.db = datacontext;
            this.mp = mapper;
            this._IRoleRespositories = IRoleRespositories;
        }

        public async Task<bool> CheckPermission(string roleName, string functionName)
        {
            var roleId = (await db.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName)).RoleId;
            var functionId = (await db.Functions.FirstOrDefaultAsync(x => x.FunctionName == functionName)).FunctionId;
            var entity = await db.Function_Roles.FirstOrDefaultAsync(x => x.RoleId == roleId && x.FunctionId == functionId);
            return entity.Active;
        }

        public async Task<List<Function_RoleViewModel>> GetAll(string type)
        {
            var query = from fr in db.Function_Roles
                        join f in db.Functions on fr.FunctionId equals f.FunctionId
                        join r in db.Roles on fr.RoleId equals r.RoleId
                        select new Function_RoleViewModel
                        {
                            FRID = fr.FRID,
                            FunctionId = fr.FunctionId,
                            RoleId = fr.RoleId,
                            PermissionId = fr.PermissionId,
                            FunctionName = f.FunctionName,
                            Title = f.Title,
                            SubTitle = f.SubTitle,
                            RoleName = r.RoleName,
                            Active = fr.Active,
                            Type = f.Type
                        };
            if (string.IsNullOrWhiteSpace(type) == false)
            {
                query = query.Where(x => x.Type.Contains(type));
            }
            return await query.ToListAsync();
        }

        public async Task<List<FunctionViewModel>> GetFunctionByRoleName(string roleName)
        {
            var roleId = (await db.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName)).RoleId;
            var data = db.Function_Roles.Where(x => x.RoleId == roleId && x.Active == true);
            var query = from fr in data
                        join f in db.Functions on fr.FunctionId equals f.FunctionId
                        select new FunctionViewModel()
                        {
                            FunctionId = f.FunctionId,
                            FunctionName = f.FunctionName,
                            Title = f.Title,
                            SubTitle = f.SubTitle,
                            Type = f.Type,
                            CreatedDate = f.CreatedDate,
                            ModifyDate = f.ModifyDate,
                            CreatedBy = f.CreatedBy,
                            Status = f.Status
                        };
            return await query.ToListAsync();
        }

        public async Task<List<Function_RoleViewModel>> GetFunctionByRoleId(string RoleId)
        {
            var entity = await db.Function_Roles.Where(x => x.RoleId.ToLower().Trim() == RoleId.ToLower().Trim()).ToListAsync();
            var model = mp.Map<List<Function_RoleViewModel>>(entity);
            return model;
        }

        public async Task<bool> Insert(Function_RoleViewModel model)
        {
            model.FRID = Guid.NewGuid().ToString();
            model.Active = true;
            var entity = mp.Map<Function_Role>(model);
            await db.Function_Roles.AddAsync(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string FRID)
        {
            var entity = await db.Function_Roles.FirstOrDefaultAsync(x => x.FRID == FRID);
            db.Function_Roles.Remove(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> SetActive(Function_RoleViewModel model)
        {
            var entity = await db.Function_Roles.FirstOrDefaultAsync(x => x.FRID == model.FRID);
            entity.Active = model.Active.Value;
            db.Function_Roles.Update(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertMultiFunctionRole(FunctionRoleParams param)
        {
            try {
                //kiểm tra role để thêm mới hoặc cập nhật role
                if (string.IsNullOrWhiteSpace(param.RoleId))
                {
                    //thêm mới
                    RoleViewModel entity = new RoleViewModel();
                    entity.RoleId = Guid.NewGuid().ToString();
                    entity.RoleName = param.RoleName;
                    entity.Status = true;
                    var ketQua = await _IRoleRespositories.Insert(entity);
                    if (ketQua == null) return false;
                    param.RoleId = entity.RoleId;
                }
                else
                {
                    //cập nhật
                    RoleViewModel entity = new RoleViewModel();
                    entity.RoleId = param.RoleId;
                    entity.RoleName = param.RoleName;
                    entity.Status = true;
                    var ketQua = await _IRoleRespositories.Update(entity);
                    if (ketQua <= 0) return false;
                }

                //xóa các bản ghi trước khi thêm
                var entities = await db.Function_Roles.Where(x => x.RoleId.ToLower().Trim() == param.RoleId.ToLower().Trim()).ToListAsync();
                db.Function_Roles.RemoveRange(entities);
                await db.SaveChangesAsync();

                //thêm các bản ghi mới
                if (param.FunctionIds.Length > 0)
                {
                    List<Function_Role> LstNew = new List<Function_Role>();
                    foreach (string item in param.FunctionIds)
                    {
                        Function_Role NewItem = new Function_Role();
                        NewItem.FRID = Guid.NewGuid().ToString();
                        NewItem.FunctionId = item;
                        NewItem.RoleId = param.RoleId;
                        NewItem.Active = true;
                        NewItem.PermissionId = null;
                        LstNew.Add(NewItem);
                    }
                    db.Function_Roles.AddRange(LstNew);
                    if (await db.SaveChangesAsync() != LstNew.Count) return false;
                }

                var entityThaoTacs = await db.Function_ThaoTacs.Where(x => x.RoleId == param.RoleId).ToListAsync();
                db.Function_ThaoTacs.RemoveRange(entityThaoTacs);
                await db.SaveChangesAsync();

                if (param.ThaoTacs.Any())
                {
                    var lstFunctionThaoTacs = param.ThaoTacs.Select(x => new Function_ThaoTacViewModel()
                    {
                        FTID = Guid.NewGuid().ToString(),
                        ThaoTacId = x.ThaoTacId,
                        RoleId = x.RoleId,
                        FunctionId = x.FunctionId,
                        PermissionId = null,
                        Active = x.Active == true ? x.Active.Value : false 
                    })
                    .ToList();

                    var entityFunc_ThaoTacs = mp.Map<List<Function_ThaoTac>>(lstFunctionThaoTacs);
                    await db.Function_ThaoTacs.AddRangeAsync(entityFunc_ThaoTacs);
                    return await db.SaveChangesAsync() == param.ThaoTacs.Count;
                }

                return true;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

    }
}
