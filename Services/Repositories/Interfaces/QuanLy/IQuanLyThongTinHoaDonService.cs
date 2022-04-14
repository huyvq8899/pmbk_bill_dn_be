using DLL.Enums;
using Services.Helper;
using Services.ViewModels.QuanLy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLy
{
    public interface IQuanLyThongTinHoaDonService
    {
        Task<List<QuanLyThongTinHoaDonViewModel>> GetListByLoaiThongTinAsync(int? loaiThongTin);
        Task<List<QuanLyThongTinHoaDonViewModel>> GetListByHinhThucVaLoaiHoaDonAsync(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon);
        Task<bool> UpdateTrangThaiSuDungTruocDoAsync();
        Task<List<EnumModel>> GetLoaiHoaDonDangSuDung();
    }
}
