using ManagementServices.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IUser_RoleRespositories
    {
        Task<List<User_RoleViewModel>> GetAll();
        Task<List<User_Role_ByUserViewModel>> GetAllByUserId(string UserId);
    }
}
