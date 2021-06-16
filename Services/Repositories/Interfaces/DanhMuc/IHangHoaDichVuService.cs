using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHangHoaDichVuService
    {
        Task<List<HangHoaDichVuViewModel>> GetAllAsync(HangHoaDichVuParams @params = null);
        Task<PagedList<HangHoaDichVuViewModel>> GetAllPagingAsync(HangHoaDichVuParams @params);
        Task<HangHoaDichVuViewModel> GetByIdAsync(string id);

        Task<HangHoaDichVuViewModel> InsertAsync(HangHoaDichVuViewModel model);
        Task<bool> UpdateAsync(HangHoaDichVuViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
