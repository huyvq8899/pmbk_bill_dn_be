using Services.Helper;
using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string HoaDonDienTuId { get; set; }

        [Display(Name = "Ngày hóa đơn")]
        public DateTime? NgayHoaDon { get; set; }

        [Display(Name = "Số hóa đơn")]
        public string SoHoaDon { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

        [Display(Name = "Mẫu số")]
        public string MauSo { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }

        [IgnoreLogging]
        public MauHoaDonViewModel MauHoaDon { get; set; }

        [IgnoreLogging]
        public string KhachHangId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }

        [Display(Name = "Người mua hàng")]
        public string HoTenNguoiMuaHang { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SoDienThoaiNguoiMuaHang { get; set; }

        [Display(Name = "Email")]
        public string EmailNguoiMuaHang { get; set; }

        [Display(Name = "Tên ngân hàng")]
        public string TenNganHang { get; set; }

        [Display(Name = "Người nhận HĐ")]
        public string HoTenNguoiNhanHD { get; set; }

        [Display(Name = "Email người nhận HĐ")]
        public string EmailNguoiNhanHD { get; set; }

        [Display(Name = "Số điện thoại người nhận HĐ")]
        public string SoDienThoaiNguoiNhanHD { get; set; }

        [Display(Name = "Số tài khoản ngân hàng")]
        public string SoTaiKhoanNganHang { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel KhachHang { get; set; }

        [IgnoreLogging]
        public string HinhThucThanhToanId { get; set; }

        [Display(Name = "Hình thức thanh toán")]
        public string TenHinhThucThanhToan { get; set; }

        [IgnoreLogging]
        public HinhThucThanhToanViewModel HinhThucThanhToan { get; set; }

        [IgnoreLogging]
        public string NhanVienBanHangId { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel NhanVienBanHang { get; set; }

        [Display(Name = "Mã nhân viên")]
        public string MaNhanVienBanHang { get; set; }

        [Display(Name = "Tên nhân viên")]
        public string TenNhanVienBanHang { get; set; }

        [IgnoreLogging]
        public string LoaiTienId { get; set; }

        [IgnoreLogging]
        public LoaiTienViewModel LoaiTien { get; set; }

        [Display(Name = "Tỷ giá")]
        public decimal? TyGia { get; set; }

        [IgnoreLogging]
        public int? TrangThai { get; set; }

        [IgnoreLogging]
        public int? TrangThaiPhatHanh { get; set; }

        [IgnoreLogging]
        public string MaTraCuu { get; set; }

        [IgnoreLogging]
        public int? TrangThaiGuiHoaDon { get; set; }

        [IgnoreLogging]
        public bool? DaGuiThongBaoXoaBoHoaDon { get; set; }

        [IgnoreLogging]
        public bool? KhachHangDaNhan { get; set; }

        [IgnoreLogging]
        public int? SoLanChuyenDoi { get; set; }

        [IgnoreLogging]
        public DateTime? NgayXoaBo { get; set; }

        [IgnoreLogging]
        public string SoCTXoaBo { get; set; }

        [IgnoreLogging]
        public int TrangThaiBienBanXoaBo { get; set; }

        [IgnoreLogging]
        public string LyDoXoaBo { get; set; }

        [IgnoreLogging]
        public int? LoaiHoaDon { get; set; }

        [IgnoreLogging]
        public DateTime? NgayLap { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel NguoiLap { get; set; }

        [IgnoreLogging]
        public int? LoaiChungTu { get; set; }

        [Display(Name = "Thời hạn thanh toán")]
        public DateTime? ThoiHanThanhToan { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiaoHang { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuChiTietViewModel> HoaDonChiTiets { get; set; }

        [IgnoreLogging]
        public UserViewModel ActionUser { get; set; }

        [IgnoreLogging]
        public string ThamChieu { get; set; }

        [IgnoreLogging]
        public string TaiLieuDinhKem { get; set; }
        //public List<ThamChieuV2ViewModel> ThamChieus { get; set; }

        [IgnoreLogging]
        public string FileChuaKy { get; set; }

        [IgnoreLogging]
        public string FileDaKy { get; set; }

        [IgnoreLogging]
        public string XMLChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLDaKy { get; set; }

        /// Thay thế
        [IgnoreLogging]
        public string ThayTheChoHoaDonId { get; set; }

        [IgnoreLogging]
        public bool? DaLapHoaDonThayThe { get; set; }

        [IgnoreLogging]
        public string LyDoThayThe { get; set; }

        [IgnoreLogging]
        public string DieuChinhChoHoaDonId { get; set; }

        [IgnoreLogging]
        public int? LoaiDieuChinh { get; set; } // DLL\Enums\LoaiDieuChinhHoaDon.cs

        [IgnoreLogging]
        public string LyDoDieuChinh { get; set; }

        [IgnoreLogging]
        public string BienBanDieuChinhId { get; set; }

        [IgnoreLogging]
        public string BienBanXoaBoId { get; set; }

        ////////////////////////////////////////////////
        [Currency]
        [Display(Name = "Tổng tiền hàng")]
        public decimal? TongTienHang { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền chiết khấu")]
        public decimal? TongTienChietKhau { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thuế GTGT")]
        public decimal? TongTienThueGTGT { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thanh toán")]
        public decimal? TongTienThanhToan { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền hàng")]
        public decimal? TongTienHangQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền chiết khấu")]
        public decimal? TongTienChietKhauQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thuế GTGT")]
        public decimal? TongTienThueGTGTQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thanh toán")]
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiHoaDon { get; set; }

        [Display(Name = "Loại tiền")]
        public string MaLoaiTien { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonCanThayThe { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonBiDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiBienBanXoaBo { get; set; }

        [IgnoreLogging]
        public string Key { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuViewModel> Children { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiPhatHanh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiGuiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiDieuChinh { get; set; }

        [IgnoreLogging]
        public int? TrangThaiBienBanDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiBienBanDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? IsVND { get; set; }

        [IgnoreLogging]
        public string SoTienBangChu { get; set; }

        [IgnoreLogging]
        public LyDoDieuChinhModel LyDoDieuChinhModel { get; set; }

        [IgnoreLogging]
        public LyDoThayTheModel LyDoThayTheModel { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }

        [Display(Name = "Trường thông tin bổ sung 1")]
        public string TruongThongTinBoSung1 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 2")]
        public string TruongThongTinBoSung2 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 3")]
        public string TruongThongTinBoSung3 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 4")]
        public string TruongThongTinBoSung4 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 5")]
        public string TruongThongTinBoSung5 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 6")]
        public string TruongThongTinBoSung6 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 7")]
        public string TruongThongTinBoSung7 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 8")]
        public string TruongThongTinBoSung8 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 9")]
        public string TruongThongTinBoSung9 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 10")]
        public string TruongThongTinBoSung10 { get; set; }

        public bool IsSended { get; set; }//đánh dấu hóa đơn được chọn gửi khi phát hành

        public string GetMoTaBienBanDieuChinh()
        {
            return $"Hai bên thống nhất lập biên bản này để điều chỉnh hóa đơn có Mẫu số {MauSo} ký hiệu {KyHieu} số {SoHoaDon} ngày {NgayHoaDon.Value:dd/MM/yyyy} mã tra cứu {MaTraCuu} theo quy định.";
        }
    }
}
