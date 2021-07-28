using Services.Helper;
using Services.Helper.LogHelper;
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
        public string KyHieu { get; set; }
        public MauHoaDonViewModel MauHoaDon { get; set; }
        public string KhachHangId { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string SoDienThoaiNguoiMuaHang { get; set; }
        public string EmailNguoiMuaHang { get; set; }
        public string TenNganHang { get; set; }
        public string HoTenNguoiNhanHD { get; set; }
        public string EmailNguoiNhanHD { get; set; }
        public string SoDienThoaiNguoiNhanHD { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public DoiTuongViewModel KhachHang { get; set; }
        public string HinhThucThanhToanId { get; set; }
        public HinhThucThanhToanViewModel HinhThucThanhToan { get; set; }
        public string NhanVienBanHangId { get; set; }
        public DoiTuongViewModel NhanVienBanHang { get; set; }
        public string MaNhanVienBanHang { get; set; }
        public string TenNhanVienBanHang { get; set; }
        public string LoaiTienId { get; set; }
        public LoaiTienViewModel LoaiTien { get; set; }
        public decimal? TyGia { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public string MaTraCuu { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public bool? KhachHangDaNhan { get; set; }
        public int? SoLanChuyenDoi { get; set; }
        public DateTime? NgayXoaBo { get; set; }
        public string SoCTXoaBo { get; set; }
        public int TrangThaiBienBanXoaBo { get; set; }
        public string LyDoXoaBo { get; set; }
        public int? LoaiHoaDon { get; set; }
        public DateTime? NgayLap { get; set; }
        public DoiTuongViewModel NguoiLap { get; set; }
        public int? LoaiChungTu { get; set; }
        public List<HoaDonDienTuChiTietViewModel> HoaDonChiTiets { get; set; }
        public UserViewModel ActionUser { get; set; }
        public string ThamChieu { get; set; }
        public string TaiLieuDinhKem { get; set; }
        //public List<ThamChieuV2ViewModel> ThamChieus { get; set; }
        public string FileChuaKy { get; set; }
        public string FileDaKy { get; set; }

        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }

        /// Thay thế
        public string ThayTheChoHoaDonId { get; set; }
        public string LyDoThayThe { get; set; }
        public string DieuChinhChoHoaDonId { get; set; }
        public int? LoaiDieuChinh { get; set; } // DLL\Enums\LoaiDieuChinhHoaDon.cs
        public string LyDoDieuChinh { get; set; }
        public string BienBanDieuChinhId { get; set; }
        public string BienBanXoaBoId { get; set; }

        ////////////////////////////////////////////////
        public decimal? TongTienHang { get; set; }
        public decimal? TongTienChietKhau { get; set; }
        public decimal? TongTienThueGTGT { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienHangQuyDoi { get; set; }
        public decimal? TongTienChietKhauQuyDoi { get; set; }
        public decimal? TongTienThueGTGTQuyDoi { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        public string TenTrangThaiHoaDon { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public string MaLoaiTien { get; set; }
        public string TenHinhThucHoaDonCanThayThe { get; set; }
        public string TenHinhThucHoaDonBiDieuChinh { get; set; }
        public string TenTrangThaiBienBanXoaBo { get; set; }
        public string Key { get; set; }
        public List<HoaDonDienTuViewModel> Children { get; set; }
        public string TenTrangThaiPhatHanh { get; set; }
        public string TenTrangThaiGuiHoaDon { get; set; }
        public string TenLoaiDieuChinh { get; set; }
        public int? TrangThaiBienBanDieuChinh { get; set; }
        public string TenTrangThaiBienBanDieuChinh { get; set; }
        public bool? IsVND { get; set; }
        public string SoTienBangChu { get; set; }
        public LyDoDieuChinhModel LyDoDieuChinhModel { get; set; }
        public LyDoThayTheModel LyDoThayTheModel { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }

        public string GetMoTaBienBanDieuChinh()
        {
            return $"Hai bên thống nhất lập biên bản này để điều chỉnh hóa đơn có Mẫu số {MauSo} ký hiệu {KyHieu} số {SoHoaDon} ngày {NgayHoaDon.Value:dd/MM/yyyy} mã tra cứu {MaTraCuu} theo quy định.";
        }
    }
}
