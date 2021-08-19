using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHinhThucThanhToanService
    {
        Task<List<HinhThucThanhToanViewModel>> GetAllAsync(HinhThucThanhToanParams @params = null);
        Task<PagedList<HinhThucThanhToanViewModel>> GetAllPagingAsync(HinhThucThanhToanParams @params);
        Task<HinhThucThanhToanViewModel> GetByIdAsync(string id);
        Task<FileReturn> ExportExcelAsync(HinhThucThanhToanParams @params);

        Task<HinhThucThanhToanViewModel> InsertAsync(HinhThucThanhToanViewModel model);
        Task<bool> UpdateAsync(HinhThucThanhToanViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(HinhThucThanhToanViewModel model);
    }
}
