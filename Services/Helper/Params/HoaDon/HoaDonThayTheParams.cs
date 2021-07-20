using ManagementServices.Helper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Helper.Params.HoaDon
{
    public class HoaDonThayTheParams : PagingParams
    {
        public LoaiTrangThaiPhatHanh LoaiTrangThaiPhatHanh { get; set; }
        public LoaiTrangThaiGuiHoaDon LoaiTrangThaiGuiHoaDon { get; set; }
        public HoaDonThayTheSearch TimKiemTheo { get; set; }
    }

    public class HoaDonDieuChinhParams : PagingParams
    {
        public List<int> TrangThaiHoaDons { get; set; }
        public LoaiTrangThaiPhatHanh LoaiTrangThaiPhatHanh { get; set; }
        public LoaiTrangThaiBienBanDieuChinhHoaDon LoaiTrangThaiBienBanDieuChinhHoaDon { get; set; }
        public HoaDonThayTheSearch TimKiemTheo { get; set; }
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

    public enum LoaiTrangThaiGuiHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa gửi hóa đơn cho khách hàng")]
        ChuaGui = 0,
        [Description("Đang gửi hóa đơn cho khách hàng")]
        DangGui = 1,
        [Description("Gửi hóa đơn cho khách hàng lỗi")]
        GuiLoi = 2,
        [Description("Đã gửi hóa đơn cho khách hàng")]
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
        public int? Level { get; set; }
    }

    public class HoaDonThayTheSearch
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
    }
}
