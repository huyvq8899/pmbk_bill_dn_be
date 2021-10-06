using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Services.Repositories.Implimentations
{
    public class UserRespositories : IUserRespositories
    {
        private readonly Datacontext db;
        private readonly IMapper mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        public UserRespositories(Datacontext datacontext, IMapper mapper, IHostingEnvironment IHostingEnvironment, IHttpContextAccessor IHttpContextAccessor)
        {
            this.db = datacontext;
            this.mp = mapper;
            _hostingEnvironment = IHostingEnvironment;
            _IHttpContextAccessor = IHttpContextAccessor;

        }
        public async Task<int> Delete(Guid Id)
        {
            try
            {
                var entity = await db.Users.FirstOrDefaultAsync(x => x.UserId == Id.ToString());
                db.Users.Remove(entity);
                var rs = await db.SaveChangesAsync();
                return rs;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            //var entity = await db.Users.ToListAsync();
            //var model = mp.Map<List<UserViewModel>>(entity);
            //return model;
            var query = from ur in db.Users
                        join rl in db.Roles on ur.RoleId equals rl.RoleId into temp
                        from rl in temp.DefaultIfEmpty()
                        select new UserViewModel
                        {
                            UserId = ur.UserId,
                            UserName = ur.UserName ?? string.Empty,
                            Email = ur.Email ?? "Email not set",
                            FullName = ur.FullName ?? "Fullname not set",
                            Gender = ur.Gender,
                            Avatar = ur.Avatar == null ? string.Empty : this.GetAvatarByHost(ur.Avatar),
                            DateOfBirth = ur.DateOfBirth ?? DateTime.Parse("01/01/0001"),
                            CreatedDate = ur.CreatedDate ?? DateTime.Parse("01/01/0001"),
                            ModifyDate = ur.ModifyDate ?? DateTime.Parse("01/01/0001"),
                            CreatedBy = ur.CreatedBy ?? string.Empty,
                            Address = ur.Address ?? "Address not set",
                            Title = ur.Title ?? "Không rõ",
                            Phone = ur.Phone ?? "Phone not set",
                            Status = ur.Status,
                            RoleId = ur.RoleId,
                            RoleName = rl.RoleName ?? string.Empty,
                            IsOnline = ur.IsOnline,
                            IsAdmin = ur.IsAdmin,
                            IsNodeAdmin = ur.IsNodeAdmin
                        };
            return await query.ToListAsync();
        }
        public async Task<List<UserViewModel>> GetAllActive()
        {
            //var entity = await db.Users.Where(x => x.Status == true).ToListAsync();
            //var model = mp.Map<List<UserViewModel>>(entity);
            var query = from ur in db.Users
                        join rl in db.Roles on ur.RoleId equals rl.RoleId into temp
                        from rl in temp.DefaultIfEmpty()
                        where ur.Status == true
                        select new UserViewModel
                        {
                            UserId = ur.UserId,
                            UserName = ur.UserName ?? string.Empty,
                            Email = ur.Email ?? "Email not set",
                            FullName = ur.FullName ?? "Fullname not set",
                            Gender = ur.Gender,
                            Avatar = ur.Avatar == null ? string.Empty : this.GetAvatarByHost(ur.Avatar),
                            DateOfBirth = ur.DateOfBirth ?? DateTime.Parse("01/01/0001"),
                            CreatedDate = ur.CreatedDate ?? DateTime.Parse("01/01/0001"),
                            ModifyDate = ur.ModifyDate ?? DateTime.Parse("01/01/0001"),
                            CreatedBy = ur.CreatedBy ?? string.Empty,
                            Address = ur.Address ?? "Address not set",
                            Title = ur.Title ?? "Không rõ",
                            Phone = ur.Phone ?? "Phone not set",
                            Status = ur.Status,
                            RoleId = ur.RoleId,
                            RoleName = rl.RoleName ?? string.Empty,
                            IsOnline = ur.IsOnline,
                            IsAdmin = ur.IsAdmin,
                            IsNodeAdmin = ur.IsNodeAdmin
                        };

            return await query.ToListAsync();
        }
        public async Task<UserViewModel> GetById(string Id)
        {
            var query = from ur in db.Users
                        join rl in db.Roles on ur.RoleId equals rl.RoleId into temp
                        from rl in temp.DefaultIfEmpty()
                        where ur.UserId == Id
                        select new UserViewModel
                        {
                            UserId = ur.UserId,
                            UserName = ur.UserName ?? string.Empty,
                            Email = ur.Email,
                            FullName = ur.FullName,
                            Gender = ur.Gender,
                            Avatar = ur.Avatar == null ? string.Empty : this.GetAvatarByHost(ur.Avatar),
                            DateOfBirth = ur.DateOfBirth,
                            CreatedDate = ur.CreatedDate,
                            ModifyDate = ur.ModifyDate,
                            CreatedBy = ur.CreatedBy ?? string.Empty,
                            Address = ur.Address,
                            Title = ur.Title,
                            Phone = ur.Phone,
                            Status = ur.Status,
                            RoleId = ur.RoleId,
                            IsOnline = ur.IsOnline,
                            RoleName = rl.RoleName ?? string.Empty,
                            IsAdmin = ur.IsAdmin,
                            IsNodeAdmin = ur.IsNodeAdmin
                        };
            return await query.FirstOrDefaultAsync();
        }
        public async Task<UserViewModel> Insert(UserViewModel model)
        {
            model.UserName = model.UserName.ToTrim();
            model.Password = "123456";
            model.ConfirmPassword = "123456";
            model.UserId = Guid.NewGuid().ToString();
            model.Status = true;
            model.Password = CreateMD5.ConvertoMD5(model.Password + model.UserName.ToUpper().Trim());
            model.ConfirmPassword = CreateMD5.ConvertoMD5(model.ConfirmPassword + model.UserName.ToUpper().Trim());
            model.CreatedDate = DateTime.Now;
            var entity = mp.Map<User>(model);
            await db.Users.AddAsync(entity);
            await db.SaveChangesAsync();
            var res = mp.Map<UserViewModel>(entity);
            return res;
        }
        public async Task<bool> CheckUserName(string userName)
        {
            var rs = await db.Users.FirstOrDefaultAsync(x => x.UserName.ToString().ToUpper().ToTrim() == (userName.ToString().ToUpper().ToTrim()));
            if (rs != null) return true;
            else return false;
        }
        public async Task<bool> ChangeStatus(string userId)
        {
            var rs = await db.Users.FirstOrDefaultAsync(x => x.UserId == (userId));
            rs.Status = !rs.Status;
            db.Users.Update(rs);
            return await db.SaveChangesAsync() > 0;
        }
        public async Task<bool> CheckEmail(string email)
        {
            var rs = await db.Users.FirstOrDefaultAsync(x => x.Email.ToString().ToUpper().ToTrim() == (email.ToString().ToUpper().ToTrim()));
            if (rs != null) return true;
            else return false;
        }
        public async Task<int> Update(UserViewModel model)
        {
            var ur = await db.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            ur.FullName = model.FullName;
            ur.Title = model.Title;
            ur.DateOfBirth = model.DateOfBirth;
            ur.Email = model.Email;
            ur.Phone = model.Phone;
            ur.Address = model.Address;
            ur.IsAdmin = model.IsAdmin;
            ur.IsNodeAdmin = model.IsNodeAdmin;
            ur.ModifyDate = DateTime.Now;
            //var entity = mp.Map<User>(model);
            db.Users.Update(ur);
            var rs = await db.SaveChangesAsync();
            return rs; // 1 thanh cong, 0 that bai
        }
        public async Task<int> UpdatePassWord(UserViewModel model)
        {
            var entity = await db.Users.FindAsync(model.UserId);
            model.Password = CreateMD5.ConvertoMD5(model.Password + entity.UserName.ToUpper().Trim());
            entity.Password = model.Password;
            entity.ConfirmPassword = model.Password;
            db.Users.Update(entity);
            var rs = await db.SaveChangesAsync();
            return rs; // 1 thanh cong, 0 that bai
        }
        public async Task<PagedList<UserViewModel>> GetAllPagingAsync(PagingParams pagingParams)
        {
            var query = from p in db.Users
                        join r in db.Roles on p.RoleId equals r.RoleId into tmpRole
                        from r in tmpRole.DefaultIfEmpty()
                        select new UserViewModel
                        {
                            UserId = p.UserId,
                            Password = p.Password ?? string.Empty,
                            ConfirmPassword = p.ConfirmPassword ?? string.Empty,
                            UserName = p.UserName ?? string.Empty,
                            Email = p.Email ?? string.Empty,
                            FullName = p.FullName ?? string.Empty,
                            Gender = p.Gender,
                            Avatar = p.Avatar == null ? string.Empty : this.GetAvatarByHost(p.Avatar),
                            DateOfBirth = p.DateOfBirth,
                            CreatedDate = p.CreatedDate,
                            Address = p.Address ?? string.Empty,
                            Phone = p.Phone ?? string.Empty,
                            Status = p.Status,
                            RoleId = p.RoleId,
                            RoleName = r.RoleName,
                            Title = p.Title ?? string.Empty,
                            IsOnline = p.IsOnline,
                            IsAdmin = p.IsAdmin
                        };
            // var query = db.Phones.Where(x => x.Status == true);
            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                var keyword = pagingParams.Keyword.ToUpper().ToTrim();
                if (DateTime.TryParseExact(keyword, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    query = query.Where(x => x.DateOfBirth.Value.Day == date.Day && x.DateOfBirth.Value.Month == date.Month && x.DateOfBirth.Value.Year == date.Year);
                }
                query = query.Where(x => x.UserName.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.UserName.ToUpper().Contains(keyword) ||
                                        x.Email.ToUpper().Contains(keyword) ||
                                        x.FullName.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.FullName.ToUpper().Contains(keyword) ||
                                        x.Address.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.Address.ToUpper().Contains(keyword) ||
                                        x.Phone.Contains(keyword) ||
                                        x.RoleName.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.RoleName.ToUpper().Contains(keyword)
                                        );
            }
            if (!string.IsNullOrEmpty(pagingParams.SortValue) && !pagingParams.SortValue.Equals("null") && !pagingParams.SortValue.Equals("undefined"))
            {
                switch (pagingParams.SortKey)
                {
                    case "userName":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.UserName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.UserName);
                        }
                        break;
                    case "email":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.Email);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Email);
                        }
                        break;
                    case "fullName":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.FullName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.FullName);
                        }
                        break;
                    case "address":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.Address);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Address);
                        }
                        break;
                    case "phone":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.Phone);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.Phone);
                        }
                        break;
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
            return await PagedList<UserViewModel>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }
        public async Task<bool> DeleteAll(List<string> Ids)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in Ids)
                    {
                        var entity = await db.Users.FirstOrDefaultAsync(x => x.UserId.ToString() == item);
                        db.Remove(entity);
                    }
                    transaction.Commit();
                    return await db.SaveChangesAsync() > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public async Task<bool> CheckPass(string username, string pass)
        {
            var passMD5 = CreateMD5.ConvertoMD5(pass.Trim() + username.ToUpper().Trim());
            var entity = await db.Users.FirstOrDefaultAsync(x => x.UserName.ToUpper().Trim() == username.ToUpper().Trim());
            //if (entity == null) return false;
            if (entity.Password == passMD5) return true;
            else return false;
        }
        public async Task<int> Login(string username, string pass)
        {
            try
            {
                var checkUsername = await this.CheckUserName(username.Trim());
                if (checkUsername == true)
                {
                    var checkPass = await this.CheckPass(username, pass);
                    if (checkPass == true)
                    {
                        var entity = (await db.Users.FirstOrDefaultAsync(x => x.UserName == username.ToTrim()));
                        if (entity.Status == false)
                        {
                            return 2; // tài khoản bị khóa
                        }
                        else
                        {
                            //entity.IsOnline = true;
                            //db.Users.Update(entity);
                            //var rs = await db.SaveChangesAsync();
                            //if (rs != 1) return -2; // lỗi không xác định
                            return 1;
                        }; // đăng nhập thành công
                    }
                    else
                    {
                        return 0; // sai pass
                    }
                }
                else
                {
                    return -1; // tài khoản không tồn tại
                }
            }
            catch (Exception)
            {
            }

            return -2;
        }

        public async Task<bool> SetRole(SetRoleParam param)
        {
            var roleId = (await db.Roles.FirstOrDefaultAsync(x => x.RoleName == param.RoleName)).RoleId;
            var entity = await db.Users.FirstOrDefaultAsync(x => x.UserId == param.UserId);
            entity.RoleId = roleId;
            db.Users.Update(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> SetOnline(string userId, bool isOnline)
        {
            var entity = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            entity.IsOnline = isOnline;
            entity.ModifyDate = DateTime.Now;
            db.Users.Update(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<UserViewModel> GetByUserName(string UserName)
        {
            //var listNg = await db.TaiKhoanNgamDinhs.ToListAsync();
            //string str = JsonConvert.SerializeObject(listNg);
            //var ids = listNg.Select(x => x.TaiKhoanNgamDinhId).ToList();
            //var listCt = await db.TaiKhoanNgamDinhChiTiets.ToListAsync();
            //var listMap = mp.Map<List<TaiKhoanNgamDinhChiTietViewModelXXX>>(listCt);
            //string str1 = JsonConvert.SerializeObject(listMap);

            var query = from ur in db.Users
                        join rl in db.Roles on ur.RoleId equals rl.RoleId into temp
                        from rl in temp.DefaultIfEmpty()
                        where ur.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                        select new UserViewModel
                        {
                            UserId = ur.UserId,
                            UserName = ur.UserName ?? string.Empty,
                            Email = ur.Email ?? "Email not set",
                            FullName = ur.FullName ?? "Fullname not set",
                            Gender = ur.Gender,
                            Avatar = ur.Avatar == null ? string.Empty : this.GetAvatarByHost(ur.Avatar),
                            DateOfBirth = ur.DateOfBirth ?? DateTime.Parse("01/01/0001"),
                            CreatedDate = ur.CreatedDate ?? DateTime.Parse("01/01/0001"),
                            ModifyDate = ur.ModifyDate ?? DateTime.Parse("01/01/0001"),
                            CreatedBy = ur.CreatedBy ?? string.Empty,
                            Address = ur.Address ?? "Address not set",
                            Title = ur.Title ?? "Không rõ",
                            Phone = ur.Phone ?? "Phone not set",
                            Status = ur.Status,
                            RoleId = ur.RoleId,
                            IsOnline = ur.IsOnline,
                            RoleName = rl.RoleName ?? string.Empty,
                            IsAdmin = ur.IsAdmin,
                            IsNodeAdmin = ur.IsNodeAdmin
                        };
            var res = await query.FirstOrDefaultAsync();
            return res;
        }

        public async Task<ResultParam> UpdateAvatar(IList<IFormFile> files, string fileName, string fileSize, string userId)
        {
            var entity = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            var upload = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
            var fileUrl = upload.InsertFileAvatar(out string name, files);
            if (!String.IsNullOrEmpty(fileUrl))
            {
                // xóa avatar cũ
                if (entity.Avatar != null)
                {
                    var rsDeleteFile = upload.DeleteFileAvatar(entity.Avatar);
                    if (rsDeleteFile != true) return new ResultParam
                    {
                        Result = false,
                        User = null

                    };
                }
                // thêm avatar mới
                try
                {
                    FileInfo file = new FileInfo(fileUrl);
                    // Lưu vào csdl
                    entity.Avatar = name;
                    db.Users.Update(entity);
                    var rs = await db.SaveChangesAsync() > 0;
                    if (rs != true)
                    {
                        return new ResultParam
                        {
                            Result = rs,
                            User = null

                        };
                    }
                    entity.Avatar = this.GetAvatarByHost(entity.Avatar);
                    return new ResultParam
                    {
                        Result = rs,
                        User = mp.Map<UserViewModel>(entity)

                    };
                }
                catch (Exception)
                {
                    FileInfo fileInfo = new FileInfo(fileUrl);
                    fileInfo.Delete();
                }
            }

            return new ResultParam
            {
                Result = false,
                User = null
            };
        }
        public string GetAvatarByHost(string avatar)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var filename = $"FilesUpload/{databaseName}/Avatar/" + avatar;
            // string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
            //string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload";
            //string filePath = Path.Combine(folder, filename);
            //string url = _IConfiguration["FolderFileBase:wework"] + filename;
            string url;
            if (_IHttpContextAccessor.HttpContext.Request.IsHttps)
            {
                url = "https://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            url = url + "/" + filename;

            return url;
        }

        public async Task<List<string>> GetPermissionByUserName(string UserName)
        {
            var queryFunctionRole = await (from table1 in db.User_Roles
                                           join table2 in db.Function_Roles on table1.RoleId equals table2.RoleId
                                           join table3 in db.Functions on table2.FunctionId equals table3.FunctionId
                                           join table4 in db.Users on table1.UserId equals table4.UserId
                                           where table4.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                                           select new FunctionViewModel
                                           {
                                               FunctionId = table2.FunctionId,
                                               FunctionName = table3.FunctionName
                                           }).ToListAsync();

            var queryFunctionUser = await (from table1 in db.Function_Users
                                           join table2 in db.Functions on table1.FunctionId equals table2.FunctionId
                                           join table3 in db.Users on table1.UserId equals table3.UserId
                                           where table3.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                                           select new FunctionViewModel
                                           {
                                               FunctionId = table2.FunctionId,
                                               FunctionName = table2.FunctionName
                                           }).ToListAsync();
            var query = queryFunctionRole.Union(queryFunctionUser).Select(x => x.FunctionName).ToList();
            return query;
        }

        public async Task<PermissionUserMViewModel> GetPermissionByUserName_new(string UserName)
        {
            var result = new PermissionUserMViewModel();

            var userId = db.Users.Where(x => x.UserName == UserName).Select(x => x.UserId).FirstOrDefault();
            var user = db.Users.FirstOrDefault(c => c.UserId == userId);
            var queryFunctionRole = await (from table1 in db.User_Roles
                                           join table2 in db.Function_Roles on table1.RoleId equals table2.RoleId
                                           join table3 in db.Functions on table2.FunctionId equals table3.FunctionId
                                           join table4 in db.Users on table1.UserId equals table4.UserId
                                           where table4.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                                           select new FunctionViewModel
                                           {
                                               FunctionId = table2.FunctionId,
                                               FunctionName = table3.FunctionName
                                           }).ToListAsync();

            var query = queryFunctionRole
                                    .DistinctBy(x => x.FunctionName)
                                    .ToList();
            var qry = query.Select(x => new PemissionUserViewModel
            {
                FunctionName = x.FunctionName,
                ThaoTacs = new List<string>()
            }).ToList();

            foreach (var item in qry)
            {
                var func = await db.Functions.Where(x => x.FunctionName == item.FunctionName).FirstOrDefaultAsync();

                item.ThaoTacs = await GetAllThaoTacOfUserFunction(func.FunctionId, userId);
            }

            result.Functions = qry;
            if (!user.IsAdmin.Value && !user.IsNodeAdmin.Value)
            {
                var queryFunctionMRole = await (from table1 in db.User_Roles
                                                join table2 in db.PhanQuyenMauHoaDons on table1.RoleId equals table2.RoleId
                                                join table3 in db.Users on table1.UserId equals table3.UserId
                                                where table3.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                                                select table2.MauHoaDonId
                                               )
                                               .Distinct()
                                               .ToListAsync();

                result.MauHoaDonIds = queryFunctionMRole;
            }
            else result.MauHoaDonIds = db.MauHoaDons.Select(x => x.MauHoaDonId).ToList();


            return result;
        }

        public async Task<List<string>> GetAllThaoTacOfUserFunction(string FunctionId, string UserId)
        {
            var result = new List<string>();

            var userRoles = await db.User_Roles.Where(x => x.UserId == UserId).Select(x => x.RoleId).ToListAsync();
            if (userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    var thaoTacs = await db.Function_ThaoTacs.Include(x => x.ThaoTac)
                                                        .Where(x => x.FunctionId == FunctionId && x.RoleId == role)
                                                        .Select(x => x.ThaoTac.Ma)
                                                        .ToListAsync();
                    foreach (var tt in thaoTacs)
                    {
                        if (!result.Contains(tt)) result.Add(tt);
                    }
                }
            }

            return result;
        }

        public async Task<bool> PhanQuyenUser(UserRoleParams param)
        {
            var user = await GetById(param.UserId);
            if (param.IsAdmin)
            {
                //cập nhật user
                user.IsAdmin = true;
                var ketQua = await Update(user);
                return ketQua > 0;
            }
            else if (param.IsNodeAdmin)
            {
                user.IsNodeAdmin = true;
                await Update(user);
            }
            else
            {
                user.IsAdmin = false;
                user.IsNodeAdmin = false;
                await Update(user);
            }

            //xóa các bản ghi user_role
            var entities_user = await db.User_Roles.Where(x => x.UserId.ToLower().Trim() == param.UserId.ToLower().Trim()).ToListAsync();
            db.User_Roles.RemoveRange(entities_user);
            await db.SaveChangesAsync();

            //thêm các bản ghi user_role
            if (param.RoleIds.Length > 0)
            {
                List<User_Role> LstNew = new List<User_Role>();
                foreach (string item in param.RoleIds)
                {
                    User_Role NewItem = new User_Role();
                    NewItem.URID = Guid.NewGuid().ToString();
                    NewItem.UserId = param.UserId;
                    NewItem.RoleId = item;
                    LstNew.Add(NewItem);
                }
                db.User_Roles.AddRange(LstNew);
                var ketQua = await db.SaveChangesAsync() > 0;
                if (ketQua == false) return false;
            }

            //xóa các bản ghi Function_Users
            var entities = await db.Function_Users.Where(x => x.UserId.ToLower().Trim() == param.UserId.ToLower().Trim()).ToListAsync();
            db.Function_Users.RemoveRange(entities);
            await db.SaveChangesAsync();

            //thêm các bản ghi Function_Users
            if (param.FunctionIds.Length > 0)
            {
                List<Function_User> LstNew = new List<Function_User>();
                foreach (string item in param.FunctionIds)
                {
                    Function_User NewItem = new Function_User();
                    NewItem.FUID = Guid.NewGuid().ToString();
                    NewItem.FunctionId = item;
                    NewItem.UserId = param.UserId;
                    NewItem.PermissionId = null;
                    LstNew.Add(NewItem);
                }
                db.Function_Users.AddRange(LstNew);

                var ketQua = await db.SaveChangesAsync() > 0;
                if (ketQua == false) return false;
            }

            return true;
        }

        public async Task<List<UserViewModel>> GetUserOnline()
        {
            return mp.Map<List<UserViewModel>>(await db.Users.Where(x => x.IsOnline == true).ToListAsync());
        }

        public async Task<int> CheckTrungTenDangNhap(UserViewModel user)
        {
            return await db.Users.CountAsync(x => x.UserName == user.UserName);
        }

        public async Task<UserViewModel> GetThongTinGanNhat()
        {
            return mp.Map<UserViewModel>(await db.Users.OrderByDescending(x => x.ModifyDate).ThenByDescending(x => x.CreatedDate).DefaultIfEmpty(null).FirstOrDefaultAsync());
        }
    }
}
