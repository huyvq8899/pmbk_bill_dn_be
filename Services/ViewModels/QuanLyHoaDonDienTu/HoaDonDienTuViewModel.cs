using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuViewModel : ThongTinChungViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public string MauSo { get; set; }
        public string TenMauSo { get; set; }
        public MauHoaDonViewModel MauHoaDon { get; set; }
        public string KhachHangId { get; set; }
        public string TenKhachHang { get; set; }
        public string MaSoThue { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string SoDienThoaiNguoiMuaHang { get; set; }
        public string EmailNguoiMuaHang { get; set; }
        public string TenNganHang { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public DoiTuongViewModel KhachHang { get; set; }
        public string HinhThucThanhToanId { get; set; }
        public HinhThucThanhToanViewModel HinhThucThanhToan { get; set; }
        public string NhanVienBanHangId { get; set; }
        public DoiTuongViewModel NhanVienBanHang { get; set; }
        public string LoaiTienId { get; set; }
        public LoaiTienViewModel LoaiTien { get; set; }
        public decimal? TyGia { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public string MaTraCuu { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public bool? KhachHangDaNhan { get; set; }
        public int? SoLanChuyenDoi { get; set; }
        public string LyDoXoaBo { get; set; }
        public int? LoaiHoaDon { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLapId { get; set; }
        public DoiTuongViewModel NguoiLap { get; set; }
        public int? LoaiChungTu { get; set; }
        public List<HoaDonDienTuChiTietViewModel> HoaDonChiTiets { get; set; }
        public UserViewModel ActionUser { get; set; }
        public string ThamChieu { get; set; }
        public string TaiLieuDinhKem { get; set; }
        //public List<ThamChieuV2ViewModel> ThamChieus { get; set; }
    }
}
