using DLL.Entity.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongTinHoaDonService
    {
        Task<ThongTinHoaDon> InsertAsync(ThongTinHoaDon model);
        Task<ThongTinHoaDon> UpdateAsync(ThongTinHoaDon model);
        Task<ThongTinHoaDonViewModel> CheckTrungThongTinAsync(ThongTinHoaDon param);
        Task<HoaDonDienTuViewModel> GetById(string Id);
        Task<bool> CheckTrungHoaDonHeThongAsync(ThongTinHoaDon model);
        Task<bool> CheckTrungThongTinThayTheAsync(ThongTinHoaDon param);
        Task<bool> CheckTrungThongTinDieuChinhAsync(ThongTinHoaDon param);
    }
}
