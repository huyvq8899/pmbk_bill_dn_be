using DLL.Enums;
using Services.Helper;
using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
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
        public string BoKyHieuHoaDonId { get; set; }

        [IgnoreLogging]
        public BoKyHieuHoaDonViewModel BoKyHieuHoaDon { get; set; }

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
        public int? TrangThai { get; set; } // DLL.Enums.TrangThaiHoaDon

        [IgnoreLogging]
        public int? TrangThaiQuyTrinh { get; set; } // DLL.Enums.TrangThaiQuyTrinh

        [IgnoreLogging]
        public string MaTraCuu { get; set; }

        [IgnoreLogging]
        public int? TrangThaiGuiHoaDon { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
        [IgnoreLogging]
        public int? TrangThaiGuiHoaDonNhap { get; set; } // DLL.Enums.TrangThaiGuiHoaDon

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
        public int? LoaiHoaDon { get; set; } // DLL.Enums.LoaiHoaDon

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

        [IgnoreLogging]
        public string MaCuaCQT { get; set; }

        [IgnoreLogging]
        public bool? IsCapMa { get; set; } // = true khi nhận được thông điệp phản hồi 202 (Không dùng trong getbyid, vv...)

        [IgnoreLogging]
        public DateTime? NgayKy { get; set; }

        [IgnoreLogging]
        public string DataXML { get; set; }

        [IgnoreLogging]
        public bool? IsPhatHanh { get; set; }

        [IgnoreLogging]
        public bool? IsReloadSignedPDF { get; set; } // reload pdf đã ký nếu bị lỗi hiển thị

        [IgnoreLogging]
        public bool? IsCoSoHoaDon { get; set; }

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
        public string Loai { get; set; }

        [IgnoreLogging]
        public string LyDoDieuChinh { get; set; }

        [IgnoreLogging]
        public bool DaBiDieuChinh { get; set; }

        [IgnoreLogging]
        public bool DaLapDieuChinh { get; set; }

        [IgnoreLogging]
        public string BienBanDieuChinhId { get; set; }

        public string BienBanDieuChinhIdTmp { get; set; }

        [IgnoreLogging]
        public string BienBanXoaBoId { get; set; }

        [IgnoreLogging]
        public bool? IsHoaDonCoMa { get; set; }

        [IgnoreLogging]
        public string HinhThucDieuChinh { get; set; }

        public bool? BlockPhatHanhLai { get; set; }

        [IgnoreLogging]
        public string TrangThaiThoaThuan { get; set; }

        [IgnoreLogging]
        public bool? IsLapVanBanThoaThuan { get; set; }

        [IgnoreLogging]
        public LoaiChietKhau LoaiChietKhau { get; set; }

        [IgnoreLogging]
        public int? IntSoHoaDon { get; set; }

        [IgnoreLogging]
        public bool? IsSentCQT { get; set; }

        [IgnoreLogging]
        public bool? IsBuyerSigned { get; set; }

        public bool BuyerSigned { get; set; }

        public DateTime? NgayNguoiMuaKy { get; set; }

        [IgnoreLogging]
        public int? SoLanGuiCQT { get; set; }

        [IgnoreLogging]
        public bool? IsHoaDonChoTCCNTKPTQ { get; set; } // hóa đơn dành cho tổ chức cá nhân trong khu phi thuế quan

        [IgnoreLogging]
        public bool? IsLapBienBanDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? IsLapHoaDonDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? IsLapHoaDonThayThe { get; set; }

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

        [Currency]
        public decimal? TongTienGiam { get; set; }

        [Currency]
        public decimal? TongTienGiamQuyDoi { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiHoaDon { get; set; }

        [Display(Name = "Loại tiền")]
        public string MaLoaiTien { get; set; }

        [IgnoreLogging]
        public int? LoaiApDungHoaDonCanThayThe { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonCanThayThe { get; set; }

        [IgnoreLogging]
        public int? LoaiApDungHoaDonDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonBiDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiBienBanXoaBo { get; set; }

        [IgnoreLogging]
        public string Key { get; set; }

        [IgnoreLogging]
        public int? HinhThucHoaDon { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDon { get; set; }

        [IgnoreLogging]
        public int? UyNhiemLapHoaDon { get; set; }

        [IgnoreLogging]
        public string TenUyNhiemLapHoaDon { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuViewModel> Children { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiQuyTrinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiPhatHanh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiGuiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiDieuChinh { get; set; }

        [IgnoreLogging]
        public int? TrangThaiBienBanDieuChinh { get; set; }
        public int? TrangThaiBienBanDieuChinhTmp { get; set; }
        [IgnoreLogging]
        public string TenTrangThaiBienBanDieuChinh { get; set; }
        public string TenTrangThaiBienBanDieuChinhTmp { get; set; }

        [IgnoreLogging]
        public bool? DaDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? LapTuPMGP { get; set; }


        [IgnoreLogging]
        public bool? IsVND { get; set; }

        [IgnoreLogging]
        public string SoTienBangChu { get; set; }

        [IgnoreLogging]
        public string ThongTinTao { get; set; }

        [IgnoreLogging]
        public string ThongTinCapNhat { get; set; }

        [IgnoreLogging]
        public LyDoDieuChinhModel LyDoDieuChinhModel { get; set; }

        public LyDoDieuChinhModel LyDoDieuChinhModelTmp { get; set; }

        [IgnoreLogging]
        public LyDoThayTheModel LyDoThayTheModel { get; set; }

        [IgnoreLogging]
        public TTChungThongDiep TTChungThongDiep { get; set; }

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

        [Display(Name = "Giảm theo nghị quyết")]
        public bool? IsGiamTheoNghiQuyet { get; set; }

        [Display(Name = "Tỷ lệ % doanh thu")]
        public decimal? TyLePhanTramDoanhThu { get; set; }

        public bool IsSended { get; set; }//đánh dấu hóa đơn được chọn gửi khi phát hành
        public bool? IsNotCreateThayThe { get; set; }//đánh dấu Hóa đơn xóa bỏ không cần lập thay thế
        public int? HinhThucXoabo { get; set; }
        public int? BackUpTrangThai { get; set; }

        [IgnoreLogging]
        public bool? HoaDonNgoaiHeThong { get; set; } //đánh dấu là hóa đơn ngoài hệ thống

        [IgnoreLogging]
        public string DienGiaiTrangThaiHoaDon { get; set; } //diễn giải thêm về trạng thái hóa đơn

        public string GetMoTaBienBanDieuChinh()
        {
            return $"Hai bên thống nhất lập biên bản này để điều chỉnh hóa đơn có Mẫu số {MauSo} ký hiệu {KyHieu} số {SoHoaDon} ngày {NgayHoaDon.Value:dd/MM/yyyy} mã tra cứu {MaTraCuu} theo quy định.";
        }

        [IgnoreLogging]
        public CotThongBaoSaiSotViewModel ThongBaoSaiSot { get; set; }

        [IgnoreLogging]
        public string ThongDiepGuiCQTId { get; set; }

        [IgnoreLogging]
        public bool? IsChildThayThe { get; set; }

        [IgnoreLogging]
        public bool? HoaDonThayTheDaDuocCapMa { get; set; }

        [IgnoreLogging]
        public bool? HoaDonDieuChinhDaDuocCapMa { get; set; }

        [IgnoreLogging]
        public string MaThongDiep { get; set; }

        [IgnoreLogging]
        public string IdHoaDonSaiSotBiThayThe { get; set; }

        [IgnoreLogging]
        public string GhiChuThayTheSaiSot { get; set; }

        [IgnoreLogging]
        public string FilterThongBaoSaiSot { get; set; }
    }

    public class CotThongBaoSaiSotViewModel
    {
        public int? TrangThaiLapVaGuiThongBao { get; set; } //trạng thái đã lập và gửi thông báo 04
        public string LanGui { get; set; }
        public string DienGiaiChiTietTrangThai { get; set; }
        public bool? IsTrongHan { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public bool? IsCoGuiEmailSaiThongTin { get; set; }
        public bool? IsHoaDonDieuChinh { get; set; }
        public bool? IsHoaDonNgoaiHeThong { get; set; } //là những hóa đơn được nhập từ phần mềm khác
        public string TenTrangThai { get; set; }
    }

    public class KetQuaKiemTraLapTBao04ViewModel
    {
        public bool IsDaLapThongBao { get; set; }
        public bool IsDaGuiThongBao { get; set; }
    }

    public class CayThayTheViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string HoaDonDienTuChaId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class ThongKeSoLuongHoaDonCoSaiSotViewModel
    {
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int SoLuong { get; set; }
        public bool IsDaLapThongBao04 { get; set; }
    }

    public class HoaDonKhongHopLeViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
    }

    public class HoaDonDaLapThongBao04ViewModel
    {
        public DLL.Entity.QuanLyHoaDon.ThongTinHoaDon HoaDonNgoaiHeThong { get; set; }
        public DLL.Entity.QuanLyHoaDon.HoaDonDienTu HoaDon { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
    }
}
