using Services.ViewModels.Ticket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Ticket
{
    public interface IUser_XeService
    {
        Task<List<User_XeViewModel>> GetListPermissionAsync();
        Task<bool> SavePermissionAsync(User_XeViewModel model);
        Task<List<string>> GetListXeIdByClaimUserIdAsync();
    }
}
