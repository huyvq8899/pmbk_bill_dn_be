using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;

namespace DLL.Entity.QuanLyHoaDon
{
    public class HoaDonDienTu : ThongTinChung
    {
        public string HoaDonDienTuId { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string KhachHangId { get; set; }
        public DoiTuong KhachHang { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string SoDienThoaiNguoiMuaHang { get; set; }
        public string EmailNguoiMuaHang { get; set; }
        public string TenNganHang { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public string HoTenNguoiNhanHD { get; set; }
        public string EmailNguoiNhanHD { get; set; }
        public string SoDienThoaiNguoiNhanHD { get; set; }
        public string HinhThucThanhToanId { get; set; }
        public HinhThucThanhToan HinhThucThanhToan { get; set; }
        public string NhanVienBanHangId { get; set; }
        public DoiTuong NhanVienBanHang { get; set; }
        public string MaNhanVienBanHang { get; set; }
        public string TenNhanVienBanHang { get; set; }
        public string LoaiTienId { get; set; }
        public LoaiTien LoaiTien { get; set; }
        public decimal? TyGia { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public string MaTraCuu { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public bool? DaGuiThongBaoXoaBoHoaDon { get; set; }
        public bool? KhachHangDaNhan { get; set; }
        public int? SoLanChuyenDoi { get; set; }
        public DateTime? NgayXoaBo { get; set; }
        public string SoCTXoaBo { get; set; }
        public int TrangThaiBienBanXoaBo { get; set; }
        public string LyDoXoaBo { get; set; }
        public int LoaiHoaDon { get; set; }
        public int LoaiChungTu { get; set; }
        public DateTime? ThoiHanThanhToan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public List<TruongMoRongHoaDon> TruongMoRongHoaDons { get; set; }
        public List<HoaDonDienTuChiTiet> HoaDonChiTiets { get; set; }
        public string ThamChieu { get; set; }
        public string TaiLieuDinhKem { get; set; }
        public string FileChuaKy { get; set; }
        public string FileDaKy { get; set; }
        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }
        /// Thay thế, điều chỉnh
        public string ThayTheChoHoaDonId { get; set; }
        public string LyDoThayThe { get; set; }
        public string DieuChinhChoHoaDonId { get; set; }
        public int? LoaiDieuChinh { get; set; } // DLL\Enums\LoaiDieuChinhHoaDon.cs
        public string LyDoDieuChinh { get; set; }
        //////////////////
        public string NguoiLapId { get; set; }
        public virtual DoiTuong NguoiLap { get; set; }
        public DateTime? NgayLap { get; set; }

        ///////////////////////////////////
        public decimal? TongTienHang { get; set; }
        public decimal? TongTienChietKhau { get; set; }
        public decimal? TongTienThueGTGT { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienHangQuyDoi { get; set; }
        public decimal? TongTienChietKhauQuyDoi { get; set; }
        public decimal? TongTienThueGTGTQuyDoi { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        //trường bổ sung
        public string TruongThongTinBoSung1Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung1 { get; set; }
        public string TruongThongTinBoSung2Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung2 { get; set; }
        public string TruongThongTinBoSung3Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung3 { get; set; }
        public string TruongThongTinBoSung4Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung4 { get; set; }
        public string TruongThongTinBoSung5Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung5 { get; set; }
        public string TruongThongTinBoSung6Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung6 { get; set; }
        public string TruongThongTinBoSung7Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung7 { get; set; }
        public string TruongThongTinBoSung8Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung8 { get; set; }
        public string TruongThongTinBoSung9Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung9 { get; set; }
        public string TruongThongTinBoSung10Id { get; set; }
        public TruongDuLieuMoRong TruongThongTinBoSung10 { get; set; }
    }
}
