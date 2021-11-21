using DLL.Entity.DanhMuc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SoLuong { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DonGia { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DonGiaSauThue { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DonGiaQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ThanhTien { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ThanhTienSauThue { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ThanhTienQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ThanhTienSauThueQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TyLeChietKhau { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TienChietKhau { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TienChietKhauQuyDoi { get; set; }
        public string ThueGTGT { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TienThueGTGT { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TienThueGTGTQuyDoi { get; set; }
        public string SoLo { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string XuatBanPhi { get; set; }
        public string GhiChu { get; set; }
        public string NhanVienBanHangId { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienThanhToan { get; set; }
        [Column(TypeName = "decimal(18,4)")]
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
