using ManagementServices.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IRoleRespositories
    {
        Task<List<RoleViewModel>> GetAll();
        Task<RoleViewModel> GetById(string Id);
        Task<RoleViewModel> Insert(RoleViewModel model);
        Task<int> Delete(string Id);
        Task<int> Update(RoleViewModel model);
        Task<PagedList<RoleViewModel>> GetAllPaging(PagingParams pagingParams);
        Task<int> CheckTrungMaWithObjectInput(RoleViewModel role);
        Task<bool> CheckPhatSinh(string roleID);
    }
}
