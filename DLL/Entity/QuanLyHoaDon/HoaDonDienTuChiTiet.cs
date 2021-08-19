using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

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
        public bool? HangKhuyenMai { get; set; }
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
        public string SoLo { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string GhiChu { get; set; }
        public string NhanVienBanHangId { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        public string TruongMoRongChiTiet1Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet1 { get; set; }
        public string TruongMoRongChiTiet2Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet2 { get; set; }
        public string TruongMoRongChiTiet3Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet3 { get; set; }
        public string TruongMoRongChiTiet4Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet4 { get; set; }
        public string TruongMoRongChiTiet5Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet5 { get; set; }
        public string TruongMoRongChiTiet6Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet6 { get; set; }
        public string TruongMoRongChiTiet7Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet7 { get; set; }
        public string TruongMoRongChiTiet8Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet8 { get; set; }
        public string TruongMoRongChiTiet9Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet9 { get; set; }
        public string TruongMoRongChiTiet10Id { get; set; }
        public TruongDuLieuMoRong TruongMoRongChiTiet10 { get; set; }

    }
}
