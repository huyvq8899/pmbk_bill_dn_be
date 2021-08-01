using AutoMapper;
using DLL;
using DLL.Entity;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class FunctionRespositories : IFunctionRespositories
    {
        Datacontext db;
        IMapper mp;
        IFunction_RoleRespositories _Function_RoleRespositories;
        IFunction_UserRespositories _Function_UserRespositories;

        public FunctionRespositories(Datacontext datacontext, IMapper mapper,
            IFunction_RoleRespositories Function_RoleRespositories,
            IFunction_UserRespositories Function_UserRespositories
            )
        {
            this.db = datacontext;
            this.mp = mapper;
            this._Function_RoleRespositories = Function_RoleRespositories;
            this._Function_UserRespositories = Function_UserRespositories;
        }

        public async Task<bool> Delete(string functionId)
        {
            //var listFR = await db.Function_Roles.Where(x => x.FunctionId == functionId).ToListAsync();
            //db.Function_Roles.RemoveRange(listFR);
            var eFunction = await db.Functions.FirstOrDefaultAsync(x => x.FunctionId == functionId);
            db.Functions.Remove(eFunction);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Insert(FunctionViewModel model)
        {
            // thêm vào bảng function
            model.FunctionId = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifyDate = model.CreatedDate;
            var entity = mp.Map<Function>(model);
            await db.Functions.AddAsync(entity);

            /* bỏ thêm vào functionRole
            var listRole = await db.Roles.ToListAsync();
            foreach (var item in listRole)
            {
                var mFR = new Function_RoleViewModel()
                {
                    FRID = Guid.NewGuid().ToString(),
                    FunctionId = entity.FunctionId,
                    RoleId = item.RoleId,
                };
                if (item.RoleName == "OWNER")
                {
                    mFR.Active = true;
                }
                var eMR = mp.Map<Function_Role>(mFR);
                await db.Function_Roles.AddAsync(eMR);
            }
            */
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<List<ThaoTacViewModel>> GetThaoTacOfFunction(string FunctionId, string RoleId, List<string> selectedFunctionIds = null)
        {
            var result = new List<ThaoTacViewModel>();
            try
            {
                var selectedFunctionId = String.Join(",", selectedFunctionIds.ToArray());
                List<SqlParameter> prm = new List<SqlParameter>()
                {
                     new SqlParameter("@FunctionId", SqlDbType.NVarChar) {Value = FunctionId},
                     new SqlParameter("@roleId", SqlDbType.NVarChar) {Value = RoleId },
                     new SqlParameter("@selectedFunctionId", SqlDbType.NVarChar) {Value = selectedFunctionId },
                };

                result = db.ViewThaoTacs.FromSql("select * from dbo.fn_getThaoTacs(@FunctionId, @roleId, @selectedFunctionId)", prm.ToArray())
                                                                .Select(x=>new ThaoTacViewModel
                                                                {
                                                                    ThaoTacId = x.ThaoTacId,
                                                                    FunctionId = x.FunctionId,
                                                                    RoleId = x.RoleId,
                                                                    PemissionId = x.PemissionId,
                                                                    FTID = x.FTID,
                                                                    Ma = x.Ma,
                                                                    Ten = x.Ten,
                                                                    STT = x.STT,
                                                                    Active = x.Active
                                                                })
                                                                .DistinctBy(x => x.ThaoTacId)
                                                                .ToList();

                if(!result.Any(x=>x.Active == true) && db.Function_Roles.Any(x=>x.FunctionId == FunctionId && x.RoleId == RoleId && x.Active == true))
                {
                    foreach(var item in result)
                    {
                        item.Active = true;
                    }
                }
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return result;
        }

        public async Task<bool> InsertUpdateThaoTacToFunction(Function_ThaoTacViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.FTID))
                {
                    model.FTID = Guid.NewGuid().ToString();
                    var entity = mp.Map<Function_ThaoTac>(model);
                    await db.AddAsync(entity);
                }
                else
                {
                    var isExist = await db.Function_ThaoTacs.AnyAsync(x => x.FTID == model.FTID);
                    var entity = mp.Map<Function_ThaoTac>(model);
                    if (isExist) db.Update(entity);
                    else await db.AddAsync(entity);
                }
                return await db.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<bool> InsertUpdateMultipleThaoTacToFunction(List<Function_ThaoTacViewModel> listThaoTac)
        {
            try
            {
                var listExist = listThaoTac.Where(x => db.Function_ThaoTacs.Select(o => o.FTID).ToList().Contains(x.FTID)).ToList();
                var listNotExist = listThaoTac.Where(x => (!db.Function_ThaoTacs.Select(o => o.FTID).ToList().Contains(x.FTID)) || string.IsNullOrEmpty(x.FTID)).ToList();

                if (listNotExist.Any())
                {
                    foreach (var model in listNotExist)
                    {
                       if(string.IsNullOrEmpty(model.FTID)) model.FTID = Guid.NewGuid().ToString();
                    }
                    var listEntity = mp.Map<List<Function_ThaoTac>>(listNotExist);
                    await db.AddRangeAsync(listEntity);
                }

                if (listExist.Any())
                {
                    var listEntity = mp.Map<List<Function_ThaoTac>>(listExist);
                    db.Function_ThaoTacs.UpdateRange(listEntity);
                }

                return await db.SaveChangesAsync() == listThaoTac.Count;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<TreeOfFunction> GetAllForTreeByRole(string RoleId)
        {
            var dsFunction = await _Function_RoleRespositories.GetFunctionByRoleId(RoleId);

            List<FunctionByTreeViewModel> ListFunctionByTreeViewModel = new List<FunctionByTreeViewModel>();
            var query = await db.Functions.Where(x => x.Status == true).OrderBy(x => x.STT).ToListAsync();

            foreach (Function item in query)
            {
                if (string.IsNullOrWhiteSpace(item.Title))
                {
                    item.Title = "";
                }
                if (string.IsNullOrWhiteSpace(item.Type))
                {
                    item.Type = "";
                }
            }

            var selectedFunctionIds = dsFunction.Select(x => x.FunctionId).ToList();
            //var distinctType = query.Select(x => x.Type).Distinct();
            //foreach (string type in distinctType)
            //{
            //    var rootRowByType = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title == "");
            //    foreach (Function item in rootRowByType)
            //    {
            //        //tạo gốc thứ 1
            //        FunctionByTreeViewModel root = new FunctionByTreeViewModel();
            //        root.Title = item.SubTitle;
            //        root.FunctionId = item.FunctionId;
            //        root.FunctionName = item.FunctionName;
            //        root.ParentFunctionId = null;
            //        if(await db.Functions.CountAsync(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item.SubTitle.ToLower().Trim()) == 0)
            //        {
            //            root.ThaoTacs = await GetThaoTacOfFunction(item.FunctionId, RoleId, selectedFunctionIds);
            //        }
            //        root.SuDung = (dsFunction.Where(x => x.FunctionId == item.FunctionId).Count() > 0);
            //        ListFunctionByTreeViewModel.Add(root);
                    
            //        //duyệt đến mục title để tạo gốc thứ 2
            //        var rootRowByTitle = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item.SubTitle.ToLower().Trim());
            //        foreach (Function item2 in rootRowByTitle)
            //        {
            //            FunctionByTreeViewModel root2 = new FunctionByTreeViewModel();
            //            root2.Title = item2.SubTitle;
            //            root2.FunctionId = item2.FunctionId;
            //            root2.FunctionName = item2.FunctionName;
            //            root2.ParentFunctionId = item.FunctionId;
            //            var lstTrung = await db.Functions.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item2.SubTitle.ToLower().Trim()).ToListAsync();
            //            if (lstTrung.Count == 0)
            //            {
            //                root2.ThaoTacs = await GetThaoTacOfFunction(item2.FunctionId, RoleId, selectedFunctionIds);
            //            }
            //            root2.SuDung = (dsFunction.Where(x => x.FunctionId == item2.FunctionId).Count() > 0);
            //            ListFunctionByTreeViewModel.Add(root2);

            //            //duyệt tiếp đến nút node (các menuitem cuối cùng)
            //            var nodeRow = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item2.SubTitle.ToLower().Trim());
            //            foreach (Function item3 in nodeRow)
            //            {
            //                FunctionByTreeViewModel node = new FunctionByTreeViewModel();
            //                node.Title = item3.SubTitle;
            //                node.FunctionId = item3.FunctionId;
            //                node.FunctionName = item3.FunctionName;
            //                node.ParentFunctionId = item2.FunctionId;
            //                node.ThaoTacs = await GetThaoTacOfFunction(item3.FunctionId, RoleId, selectedFunctionIds);
            //                node.SuDung = (dsFunction.Where(x => x.FunctionId == item3.FunctionId).Count() > 0);
            //                ListFunctionByTreeViewModel.Add(node);
            //            }
            //        }
            //    }
            //}

            TreeOfFunction result = new TreeOfFunction();
            result.FunctionByTreeViewModel = await GetTreeOfFunctions(RoleId);
            result.SelectedFunctions = (from item in dsFunction
                                        select new FunctionViewModel { FunctionId = item.FunctionId }).ToList();
            return result;
        }

        public async Task<TreeOfFunction> GetAllForTreeByUser(string UserId)
        {
            var dsFunction = await _Function_UserRespositories.GetFunctionByUserId(UserId);

            List<FunctionByTreeViewModel> ListFunctionByTreeViewModel = new List<FunctionByTreeViewModel>();
            var user = mp.Map<UserViewModel>(await db.Users.FirstOrDefaultAsync(x => x.UserId == UserId));
            var query = await db.Functions.Where(x => x.Status == true).OrderBy(x => x.STT).ToListAsync();

            var selectedFunctionIds = dsFunction.Select(x => x.FunctionId).ToList();

            foreach (Function item in query)
            {
                if (string.IsNullOrWhiteSpace(item.Title))
                {
                    item.Title = "";
                }
                if (string.IsNullOrWhiteSpace(item.Type))
                {
                    item.Type = "";
                }
            }

            var distinctType = query.Select(x => x.Type).Distinct();
            foreach (string type in distinctType)
            {
                var rootRowByType = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title == "");
                foreach (Function item in rootRowByType)
                {
                    //tạo gốc thứ 1
                    FunctionByTreeViewModel root = new FunctionByTreeViewModel();
                    root.Title = item.SubTitle;
                    root.FunctionId = item.FunctionId;
                    root.FunctionName = item.FunctionName;
                    root.ParentFunctionId = null;
                    root.SuDung = (user.IsAdmin == true) ? true : (dsFunction.Where(x => x.FunctionId == item.FunctionId).Count() > 0);
                    ListFunctionByTreeViewModel.Add(root);

                    //duyệt đến mục title để tạo gốc thứ 2
                    var rootRowByTitle = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item.SubTitle.ToLower().Trim());
                    foreach (Function item2 in rootRowByTitle)
                    {
                        FunctionByTreeViewModel root2 = new FunctionByTreeViewModel();
                        root2.Title = item2.SubTitle;
                        root2.FunctionId = item2.FunctionId;
                        root2.FunctionName = item2.FunctionName;
                        root2.ParentFunctionId = item.FunctionId;
                        root2.SuDung = (user.IsAdmin == true) ? true : (dsFunction.Where(x => x.FunctionId == item2.FunctionId).Count() > 0);
                        ListFunctionByTreeViewModel.Add(root2);

                        //duyệt tiếp đến nút node (các menuitem cuối cùng)
                        var nodeRow = query.Where(x => x.Type.ToLower().Trim() == type.ToLower().Trim() && x.Title.ToLower().Trim() == item2.SubTitle.ToLower().Trim());
                        foreach (Function item3 in nodeRow)
                        {
                            FunctionByTreeViewModel node = new FunctionByTreeViewModel();
                            node.Title = item3.SubTitle;
                            node.FunctionId = item3.FunctionId;
                            node.FunctionName = item3.FunctionName;
                            node.ParentFunctionId = item2.FunctionId;
                            node.SuDung = (user.IsAdmin == true) ? true : (dsFunction.Where(x => x.FunctionId == item3.FunctionId).Count() > 0);
                            ListFunctionByTreeViewModel.Add(node);
                        }
                    }
                }
            }

            TreeOfFunction result = new TreeOfFunction();
            result.FunctionByTreeViewModel = GetTree(ListFunctionByTreeViewModel);
            result.SelectedFunctions = (from item in dsFunction
                                        select new FunctionViewModel
                                        {
                                            FunctionId = item.FunctionId,
                                        }).ToList();
            return result;
        }

        private async Task<List<FunctionByTreeViewModel>> GetTreeOfFunctions(string RoleId)
        {
            var result = new List<FunctionByTreeViewModel>();
            try
            {
                var dsFunction = await _Function_RoleRespositories.GetFunctionByRoleId(RoleId);
                var selectedFunctionIds = dsFunction.Select(x => x.FunctionId).ToList();
                var nodes = await db.Functions.ToListAsync();
                var listConvert = nodes.Select(x => new FunctionByTreeViewModel
                {
                    Title = x.SubTitle,
                    FunctionId = x.FunctionId,
                    FunctionName = x.FunctionName,
                    ParentFunctionId = nodes.Where(o => o.SubTitle == x.Title).Select(o=>o.FunctionId).FirstOrDefault(),
                    STT = x.STT
                }).OrderBy(x=>x.STT).ToList();

                foreach(var item in listConvert)
                {
                    if (!listConvert.Any(x => x.ParentFunctionId == item.FunctionId))
                    {
                        item.ThaoTacs = await GetThaoTacOfFunction(item.FunctionId, RoleId, selectedFunctionIds);
                    }
                }

                var byIdLookup = listConvert.ToLookup(i => i.FunctionId);

                foreach (var item in listConvert)
                {
                    item.Key = item.FunctionId;
                    if (item.ParentFunctionId != null)
                    {
                        var parent = byIdLookup[item.ParentFunctionId].First();
                        if (parent.Children == null)
                        {
                            parent.Children = new List<FunctionByTreeViewModel>();
                        }
                        parent.Children.Add(item);
                    }
                    else
                    {
                        if (item.IsRootTree == false) item.IsRootTree = true;
                    }
                }

                result = listConvert.Where(i => i.ParentFunctionId == null).OrderBy(x=>x.STT).ToList();
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }

        public List<FunctionByTreeViewModel> GetTree(List<FunctionByTreeViewModel> LstNodes)
        {
            var byIdLookup = LstNodes.ToLookup(i => i.FunctionId);

            foreach (var item in LstNodes)
            {
                item.Key = item.FunctionId;
                if (item.ParentFunctionId != null)
                {
                    var parent = byIdLookup[item.ParentFunctionId].First();
                    if (parent.Children == null)
                    {
                        parent.Children = new List<FunctionByTreeViewModel>();
                    }
                    parent.Children.Add(item);
                }
                else
                {
                    if (item.IsRootTree == false) item.IsRootTree = true;
                }
            }

            return LstNodes.Where(i => i.ParentFunctionId == null).ToList();
        }
    }
}
