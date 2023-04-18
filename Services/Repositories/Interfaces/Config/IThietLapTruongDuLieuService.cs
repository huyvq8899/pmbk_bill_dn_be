using DLL.Enums;
using Services.ViewModels.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Config
{
    public interface IThietLapTruongDuLieuService
    {
        Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongDuLieuByLoaiTruongAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
        Task<List<ThietLapTruongDuLieuViewModel>> GetListThietLapMacDinhAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
        Task<bool> CheckDaPhatSinhThongBaoPhatHanhAsync(ThietLapTruongDuLieuViewModel model);
        Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongMoRongByMauHoaDonIdAsync(string mauHoaDonId);
        Task<bool> InsertRangeAsync(string boKyHieuHoaDonId, List<ThietLapTruongDuLieuViewModel> models);
        Task UpdateHienThiTruongBanHangTheoDonGiaSauThuesAsync(TuyChonViewModel model);

        Task UpdateRangeAsync(List<ThietLapTruongDuLieuViewModel> models);
        Task UpdateAsync(ThietLapTruongDuLieuViewModel model);
    }
}
