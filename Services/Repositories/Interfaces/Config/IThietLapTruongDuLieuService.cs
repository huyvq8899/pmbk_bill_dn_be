using DLL.Enums;
using Services.ViewModels.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Config
{
    public interface IThietLapTruongDuLieuService
    {
        Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongDuLieuByLoaiTruongAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
        List<ThietLapTruongDuLieuViewModel> GetListThietLapMacDinh(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
        Task<bool> CheckDaPhatSinhThongBaoPhatHanhAsync(ThietLapTruongDuLieuViewModel model);

        Task UpdateRangeAsync(List<ThietLapTruongDuLieuViewModel> models);
        Task UpdateAsync(ThietLapTruongDuLieuViewModel model);
    }
}
