using DLL.Enums;
using Services.ViewModels.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Config
{
    public interface IThietLapTruongDuLieuService
    {
        Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongDuLieuByLoaiTruongAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
        Task UpdateTruongDuLieuAsync(List<ThietLapTruongDuLieuViewModel> models);
        List<ThietLapTruongDuLieuViewModel> GetListThietLapMacDinh(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon);
    }
}
