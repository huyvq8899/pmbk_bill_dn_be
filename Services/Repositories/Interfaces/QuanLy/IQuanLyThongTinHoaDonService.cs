using DLL.Entity.QuanLy;
using DLL.Entity.TienIch;
using DLL.Enums;
using Services.Helper;
using Services.ViewModels.QuanLy;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLy
{
    public interface IQuanLyThongTinHoaDonService
    {
        Task<List<QuanLyThongTinHoaDonViewModel>> GetListByLoaiThongTinAsync(int? loaiThongTin);
        Task<List<QuanLyThongTinHoaDonViewModel>> GetListByHinhThucVaLoaiHoaDonAsync(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon);
        Task<QuanLyThongTinHoaDonViewModel> GetByLoaiThongTinChiTietAsync(LoaiThongTinChiTiet loaiThongTinChiTiet);
        Task<bool> UpdateTrangThaiSuDungTruocDoAsync();
        Task<List<EnumModel>> GetLoaiHoaDonDangSuDung();
        Task<List<EnumModel>> GetHinhThucHoaDonDangSuDung();
        Task<List<SinhSoHDMayTinhTien>> GetHistorySinhSoHoaDonCMMTTien(int year);

        Task<bool> UpdateSoBatDauSinhSoHoaDonCMMTTien(long SoBatdau);

        Task<bool> UpdateSoDaSinhMoiNhatHoaDonCMMTTien(long SoCapNhat);

        Task<List<NhatKyTruyCapViewModel>> GetHistorySinhSoHoaDonCMMTTienInNhatKyTruyCap(Guid Id);

        Task<bool> UpdateSoDaSinhMoiNhatHoaDonCMMTTienByNSD(long SoCapNhat);
        Task<long> GetSoCuoiMaxMaCQTCapMTTAsync(int year);
    }
}
