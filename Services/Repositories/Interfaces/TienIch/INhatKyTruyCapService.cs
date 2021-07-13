using ManagementServices.Helper;
using Services.Helper.Params.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    public interface INhatKyTruyCapService
    {
        Task<PagedList<NhatKyTruyCapViewModel>> GetAllPagingAsync(NhatKyTruyCapParams @params);
        Task<NhatKyTruyCapViewModel> GetByIdAsync(string id);

        Task<bool> DeleteAsync(string id);
    }
}
