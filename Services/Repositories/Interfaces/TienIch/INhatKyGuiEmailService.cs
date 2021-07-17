using ManagementServices.Helper;
using Services.Helper.Params.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    public interface INhatKyGuiEmailService
    {
        Task<PagedList<NhatKyGuiEmailViewModel>> GetAllPagingAsync(NhatKyGuiEmailParams @params);
        Task<NhatKyTruyCapViewModel> GetByIdAsync(string id);

        Task<bool> InsertAsync(NhatKyTruyCapViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
