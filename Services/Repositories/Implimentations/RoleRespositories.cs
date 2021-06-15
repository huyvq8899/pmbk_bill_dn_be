﻿using AutoMapper;
using DLL;
using DLL.Entity;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class RoleRespositories : IRoleRespositories
    {
        IUserRespositories _IUserRespositories;
        Datacontext db;
        IMapper mp;
        public RoleRespositories(Datacontext datacontext, IMapper mapper, IUserRespositories _iUserRespositories)
        {
            this.db = datacontext;
            this.mp = mapper;
            this._IUserRespositories = _iUserRespositories;
        }

        public async Task<bool> CheckPhatSinh(string roleID)
        {
            return await db.User_Roles.AnyAsync(x => x.RoleId == roleID);
        }

        public async Task<int> CheckTrungMaWithObjectInput(RoleViewModel role)
        {
            return await db.Roles.CountAsync(x => x.RoleName.ToUpper() == role.RoleName.ToUpper());
        }

        public async Task<int> Delete(string Id)
        {
            try
            {
                var entity = await db.Roles.FindAsync(Id);
                db.Roles.Remove(entity);
                var rs = await db.SaveChangesAsync();
                return rs;
            }
            catch (DbUpdateException)
            {
                return -1; // khong xoa duoc khoa ngoai
            }
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var entity = await db.Roles.OrderBy(x=>x.RoleName).ToListAsync();
            var model = mp.Map<List<RoleViewModel>>(entity);
            return model;
        }

        public async Task<PagedList<RoleViewModel>> GetAllPaging(PagingParams pagingParams)
        {
            try
            {
                var query = from r in db.Roles
                            select new RoleViewModel
                            {
                                RoleId = r.RoleId,
                                RoleName = r.RoleName ?? string.Empty,
                                CreatedDate = r.CreatedDate,
                                CreatedBy = r.CreatedBy ?? string.Empty,
                                ModifyDate = r.ModifyDate,
                                Status = r.Status
                            };

                if (!string.IsNullOrEmpty(pagingParams.Keyword))
                {
                    var keyword = pagingParams.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.RoleName.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                            x.RoleName.ToUpper().Contains(keyword)
                                            );
                }
                if (!string.IsNullOrEmpty(pagingParams.SortValue) && !pagingParams.SortValue.Equals("null") && !pagingParams.SortValue.Equals("undefined"))
                {
                    switch (pagingParams.SortKey)
                    {
                        case "roleName":
                            if (pagingParams.SortValue == "descend")
                            {
                                query = query.OrderByDescending(x => x.RoleName);
                            }
                            else
                            {
                                query = query.OrderBy(x => x.RoleName);
                            }
                            break;
                        default:
                            break;
                    }
                }
                return await PagedList<RoleViewModel>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<RoleViewModel> GetById(string Id)
        {
            var entity = await db.Roles.FindAsync(Id);
            return mp.Map<RoleViewModel>(entity);
        }

        public async Task<RoleViewModel> Insert(RoleViewModel model)
        {
            model.RoleId = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifyDate = model.CreatedDate;
            var entity = mp.Map<Role>(model);
            await db.Roles.AddAsync(entity);
            var rs = await db.SaveChangesAsync();
            if(rs > 0)
            {
                return mp.Map<RoleViewModel>(entity);
            }
            return null;
        }

        public async Task<int> Update(RoleViewModel model)
        {
            var entity = mp.Map<Role>(model);
            entity.ModifyDate = DateTime.Now;
            db.Roles.Update(entity);
            var rs = await db.SaveChangesAsync();
            return rs;
        }
        private bool SetNoticeDaedLine(string DateLine)
        {
            if (!string.IsNullOrEmpty(DateLine))
            {
                if (DateTime.Parse(DateLine) < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
