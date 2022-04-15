using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Helper.Params.HeThong;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IUserRespositories
    {
        Task<PagedList<UserViewModel>> GetAllPaging(UserParams @params);
        Task<List<UserViewModel>> GetAllActive();
        Task<UserViewModel> GetById(string Id);
        Task<UserViewModel> GetByUserName(string UserName);
        Task<PermissionUserMViewModel> GetPermissionByUserName_new(string UserName);
        Task<List<string>> GetPermissionByUserName(string UserName);
        Task<bool> PhanQuyenUser(UserRoleParams param);
        Task<UserViewModel> Insert(UserViewModel model);
        Task<int> Delete(Guid Id);
        Task<int> Update(UserViewModel model);
        Task<int> UpdatePassWord(UserViewModel model);
        Task<PagedList<UserViewModel>> GetAllPagingAsync(PagingParams pagingParams);
        Task<List<UserViewModel>> GetAdminUser();
        Task<bool> CheckUserName(string userName);
        Task<bool> CheckEmail(string email);
        Task<bool> CheckPass(string username, string pass);
        Task<bool> ChangeStatus(string userId);
        Task<bool> DeleteAll(List<string> Ids);
        Task<int> Login(string username, string pass);
        Task<bool> SetRole(SetRoleParam param);
        Task<bool> SetOnline(string userId, bool isOnline);
        Task<List<UserViewModel>> GetUserOnline();
        Task<ResultParam> UpdateAvatar(IList<IFormFile> files, string fileName, string fileSize, string userId);
        string GetAvatarByHost(string avatar);
        Task<int> CheckTrungTenDangNhap(UserViewModel user);
        Task<UserViewModel> GetThongTinGanNhat();
        Task<List<string>> GetAllThaoTacOfUserFunction(string FunctionId, string UserId);
    }
}
