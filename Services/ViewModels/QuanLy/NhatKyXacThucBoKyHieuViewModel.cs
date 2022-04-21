using DLL.Enums;
using System;

namespace Services.ViewModels.QuanLy
{
    public class NhatKyXacThucBoKyHieuViewModel : ThongTinChungViewModel
    {
        public string NhatKyXacThucBoKyHieuId { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public TrangThaiSuDung TrangThaiSuDung { get; set; }
        public string NoiDung { get; set; }
        public string MauHoaDonId { get; set; }
        public string ThongDiepId { get; set; }

        public string TenNguoiXacThuc { get; set; }
        public DateTime? ThoiGianXacThuc { get; set; }
        public string TenToChucChungThuc { get; set; }
        public string SoSeriChungThu { get; set; }
        public DateTime? ThoiGianSuDungTu { get; set; }
        public DateTime? ThoiGianSuDungDen { get; set; }
        public string TenMauHoaDon { get; set; }
        public string MaThongDiepGui { get; set; }
        public DateTime? ThoiDiemChapNhan { get; set; }
        public LoaiHetHieuLuc LoaiHetHieuLuc { get; set; }
        public long? SoLuongHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public PhuongThucChuyenDL PhuongThucChuyenDL { get; set; }
        /////////////////////////////////////
        public string TenTrangThaiSuDung { get; set; }
        public string TenPhuongThucChuyenDL { get; set; }

        public BoKyHieuHoaDonViewModel BoKyHieuHoaDon { get; set; }
    }
}
