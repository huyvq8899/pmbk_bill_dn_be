using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.TienIch;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    public interface INhatKyGuiEmailService
    {
        Task<PagedList<NhatKyGuiEmailViewModel>> GetAllPagingAsync(NhatKyGuiEmailParams @params);
        Task<FileReturn> ExportExcelAsync(NhatKyGuiEmailParams @params);

        Task<bool> InsertAsync(NhatKyGuiEmailViewModel model);

        Task<bool> KiemTraDaGuiEmailChoKhachHangAsync(string hoaDonDienTuId);
        Task<HoaDonDienTuViewModel> GetThongTinById(string Id);
        Task<HoaDonDienTuViewModel> GetByIdAsync(string id);
    }
}
