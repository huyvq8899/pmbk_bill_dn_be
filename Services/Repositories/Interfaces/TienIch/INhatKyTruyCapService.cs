using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.TienIch;
using Services.ViewModels.TienIch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    public interface INhatKyTruyCapService
    {
        Task<PagedList<NhatKyTruyCapViewModel>> GetAllPagingAsync(NhatKyTruyCapParams @params);
        Task<NhatKyTruyCapViewModel> GetByIdAsync(string id);
        Task<List<NhatKyTruyCapViewModel>> GetByRefIdAsync(string id);
        Task<FileReturn> ExportExcelAsync(NhatKyTruyCapParams @params);

        Task<bool> InsertAsync(NhatKyTruyCapViewModel model);
    }
}
