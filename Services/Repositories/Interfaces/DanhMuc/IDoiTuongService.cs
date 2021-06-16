using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IDoiTuongService
    {
        Task<List<DoiTuongViewModel>> GetAllAsync(DoiTuongParams @params = null);
        Task<PagedList<DoiTuongViewModel>> GetAllPagingAsync(DoiTuongParams @params);
        Task<DoiTuongViewModel> GetByIdAsync(string id);

        Task<DoiTuongViewModel> InsertAsync(DoiTuongViewModel model);
        Task<bool> UpdateAsync(DoiTuongViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
