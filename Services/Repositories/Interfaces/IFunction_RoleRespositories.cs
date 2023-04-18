using Services.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IFunction_RoleRespositories
    {
        Task<List<Function_RoleViewModel>> GetAll(string type);
        Task<List<FunctionViewModel>> GetFunctionByRoleName(string roleName);
        Task<List<Function_RoleViewModel>> GetFunctionByRoleId(string RoleId);
        Task<bool> SetActive(Function_RoleViewModel model);
        Task<bool> CheckPermission(string roleName, string functionName);
        Task<bool> Insert(Function_RoleViewModel model);
        Task<bool> Delete(string FRID);
        Task<bool> InsertMultiFunctionRole(FunctionRoleParams param);
    }
}
