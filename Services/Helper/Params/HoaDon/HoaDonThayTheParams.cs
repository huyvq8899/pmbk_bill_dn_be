using DLL.Enums;
using ManagementServices.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Helper.Params.HoaDon
{
    public class HoaDonThayTheParams : PagingParams
    {
        public HoaDonDienTuViewModel Filter { get; set; }
        public int? LoaiTrangThaiGuiHoaDon { get; set; }
        public int? TrangThaiQuyTrinh { get; set; }
        public HoaDonSearch TimKiemTheo { get; set; }
        public string TimKiemBatKy { get; set; }
        public int? LoaiNghiepVu { get; set; }
    }

    public class HoaDonDieuChinhParams : PagingParams
    {
        public HoaDonDienTuViewModel Filter { get; set; }
        public LoaiTrangThaiHoaDonDieuChinh LoaiTrangThaiHoaDonDieuChinh { get; set; }
        public TrangThaiQuyTrinh LoaiTrangThaiPhatHanh { get; set; }
        public LoaiTrangThaiBienBanDieuChinhHoaDon LoaiTrangThaiBienBanDieuChinhHoaDon { get; set; }
        public TrangThaiGuiHoaDon TrangThaiGuiHoaDon { get; set; }
        public HoaDonSearch TimKiemTheo { get; set; }
        public bool? IsLapBienBan { get; set; }
        public int? LoaiNghiepVu { get; set; }
    }

    public enum LoaiTrangThaiPhatHanh
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa phát hành")]
        ChuaPhatHanh = 0,
        [Description("Đang phát hành")]
        DangPhatHanh = 1,
        [Description("Phát hành lỗi")]
        PhatHanhLoi = 2,
        [Description("Đã phát hành")]
        DaPhatHanh = 3,
    }

    public enum LoaiTrangThaiHoaDonDieuChinh
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Hóa đơn chưa lập điều chỉnh")]
        ChuaLap = 0,
        [Description("Hóa đơn đã lập điều chỉnh")]
        DaLap = -2,
        [Description("Hóa đơn điều chỉnh tăng")]
        DieuChinhTang = 1,
        [Description("Hóa đơn điều chỉnh giảm")]
        DieuChinhGiam = 2,
        [Description("Hóa đơn điều chỉnh thông tin")]
        DieuChinhThongTin = 3,
    }

    public enum LoaiTrangThaiGuiHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa gửi cho khách hàng")]
        ChuaGui = 0,
        [Description("Đang gửi cho khách hàng")]
        DangGui = 1,
        [Description("Gửi cho khách hàng lỗi")]
        GuiLoi = 2,
        [Description("Đã gửi cho khách hàng")]
        DaGui = 3,
    }

    public enum LoaiTrangThaiBienBanDieuChinhHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa lập biên bản")]
        ChuaLapBienBan = 0,
        [Description("Chưa ký biên bản")]
        ChuaKyBienBan = 1,
        [Description("Chưa gửi khách hàng")]
        ChuaGuiKhachHang = 2,
        [Description("Chờ khách hàng ký")]
        ChoKhachHangKy = 3,
        [Description("Khách hàng đã ký")]
        KhachHangDaKy = 4
    }

    public class TrangThaiHoaDonDieuChinh
    {
        public int? Key { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsParent { get; set; } = false;
        public int? Level { get; set; }
    }


    public class HoaDonSearch
    {
        [Display(Name = "Loại hóa đơn")]
        public string LoaiHoaDon { get; set; }
        [Display(Name = "Ký hiệu mẫu số hóa đơn")]
        public string MauSo { get; set; }
        [Display(Name = "Ký hiệu hóa đơn")]
        public string KyHieu { get; set; }
        [Display(Name = "Số hóa đơn")]
        public string SoHoaDon { get; set; }
        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; }
        [Display(Name = "Người mua hàng")]
        public string NguoiMuaHang { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        public string NgayHoaDon { get; set; }
    }

    public class ThongDiepSearch
    {
        [Display(Name = "Phiên bản")]
        public string PhienBan { get; set; }
        [Display(Name = "Mã nơi gửi")]
        public string MaNoiGui { get; set; }
        [Display(Name = "Mã nơi nhận")]
        public string MaNoiNhan { get; set; }
        [Display(Name = "Mã loại thông điệp")]
        public int? MaLoaiThongDiep { get; set; }
        [Display(Name = "Mã thông điệp")]
        public string MaThongDiep { get; set; }
        [Display(Name = "Mã thông điệp tham chiếu")]
        public string MaThongDiepThamChieu { get; set; }
        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }
        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }
    }

    public class BangTongHopSearch
    {
        [Display(Name = "Số bảng tổng hợp")]
        public int? SoBangTongHop { get; set; }
        [Display(Name = "Bổ sung lần thứ")]
        public int? BoSungLanThu { get; set; }
        [Display(Name = "Sửa đổi lần thứ")]
        public int? SuaDoiLanThu { get; set; }
    }
}

