using DLL.Enums;
using Services.ViewModels.TienIch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    public interface INhatKyThaoTacLoiService
    {
        Task<bool> InsertAsync(NhatKyThaoTacLoiViewModel model);
        Task<List<NhatKyThaoTacLoiViewModel>> GetByRefIdAsync(string refId);
        Task<List<NhatKyThaoTacLoiViewModel>> GetByDetailAsync(string refId, ThaoTacLoi thaoTacLoi);
    }
}
