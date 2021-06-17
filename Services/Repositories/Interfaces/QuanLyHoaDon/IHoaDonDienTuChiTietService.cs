using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IHoaDonDienTuChiTietService
    {
        Task<HoaDonDienTuViewModel> GetMainAndDetailByPhieuIdAsync(string phieuId);
        Task<List<HoaDonDienTuChiTietViewModel>> InsertRangeAsync(HoaDonDienTuViewModel hoaDonDienTuVM, List<HoaDonDienTuChiTietViewModel> list);
        Task RemoveRangeAsync(string HoaDonDienTuId);
    }
}
