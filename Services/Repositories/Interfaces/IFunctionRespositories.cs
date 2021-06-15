
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IFunctionRespositories
    {
        Task<bool> Insert(FunctionViewModel model);
        Task<bool> Delete(string functionId);
        Task<List<ThaoTacViewModel>> GetThaoTacOfFunction(string FunctionId, string RoleId, List<string> selectedFunctionIds = null);
        Task<bool> InsertUpdateMultipleThaoTacToFunction(List<Function_ThaoTacViewModel> listThaoTac);
        Task<bool> InsertUpdateThaoTacToFunction(Function_ThaoTacViewModel model);
        Task<TreeOfFunction> GetAllForTreeByRole(string RoleId);
        Task<TreeOfFunction> GetAllForTreeByUser(string UserId);
    }
}
