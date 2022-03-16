using DLL.Entity.DanhMuc;
using System;

namespace DLL.Entity.QuanLyHoaDon
{
    public class HoaDonDienTuChiTiet : ThongTinChung
    {
        public string HoaDonDienTuChiTietId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public HoaDonDienTu HoaDon { get; set; }
        public string HangHoaDichVuId { get; set; }
        public HangHoaDichVu HangHoaDichVu { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public bool? DongChietKhau { get; set; }
        public bool? DongMoTa { get; set; }
        public int TinhChat { get; set; }
        public string MaQuyCach { get; set; }
        public string DonViTinhId { get; set; }
        public DonViTinh DonViTinh { get; set; }
        public decimal? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? DonGiaSauThue { get; set; }
        public decimal? DonGiaQuyDoi { get; set; }
        public decimal? ThanhTien { get; set; }
        public decimal? ThanhTienSauThue { get; set; }
        public decimal? ThanhTienQuyDoi { get; set; }
        public decimal? ThanhTienSauThueQuyDoi { get; set; }
        public decimal? TyLeChietKhau { get; set; }
        public decimal? TienChietKhau { get; set; }
        public decimal? TienChietKhauQuyDoi { get; set; }
        public string ThueGTGT { get; set; }
        public decimal? TienThueGTGT { get; set; }
        public decimal? TienThueGTGTQuyDoi { get; set; }
        public decimal? TyLePhanTramDoanhThu { get; set; }
        public decimal? TienGiam { get; set; }
        public decimal? TienGiamQuyDoi { get; set; }
        public string SoLo { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string XuatBanPhi { get; set; }
        public string GhiChu { get; set; }
        public string NhanVienBanHangId { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        public string TruongMoRongChiTiet1 { get; set; }
        public string TruongMoRongChiTiet2 { get; set; }
        public string TruongMoRongChiTiet3 { get; set; }
        public string TruongMoRongChiTiet4 { get; set; }
        public string TruongMoRongChiTiet5 { get; set; }
        public string TruongMoRongChiTiet6 { get; set; }
        public string TruongMoRongChiTiet7 { get; set; }
        public string TruongMoRongChiTiet8 { get; set; }
        public string TruongMoRongChiTiet9 { get; set; }
        public string TruongMoRongChiTiet10 { get; set; }

    }
}
