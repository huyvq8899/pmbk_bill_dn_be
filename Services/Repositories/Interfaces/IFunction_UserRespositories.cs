using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IFunction_UserRespositories
    {
        Task<List<Function_UserViewModel>> GetAll();
        Task<bool> Insert(Function_UserViewModel model);
        Task<bool> Delete(string FUID);
        Task<List<Function_UserViewModel>> GetFunctionByUserId(string UserId);
    }
}
