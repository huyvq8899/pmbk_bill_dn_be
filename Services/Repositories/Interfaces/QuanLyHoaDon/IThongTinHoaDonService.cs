using DLL.Entity.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongTinHoaDonService
    {
        Task<ThongTinHoaDon> InsertAsync(ThongTinHoaDon model);
        Task<ThongTinHoaDon> UpdateAsync(ThongTinHoaDon model);
        Task<HoaDonDienTuViewModel> GetById(string Id);
    }
}
