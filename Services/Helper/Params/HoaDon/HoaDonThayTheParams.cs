using ManagementServices.Helper;
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

    public enum LoaiTrangThaiPhatHanh
    {
        [Description("Chưa phát hành")]
        ChuaPhatHanh = 1,
        [Description("Đang phát hành")]
        DangPhatHanh = 2,
        [Description("Phát hành lỗi")]
        PhatHanhLoi = 3,
        [Description("Đã phát hành")]
        DaPhatHanh = 4,
        [Description("Tất cả")]
        TatCa = 0
    }

    public enum LoaiTrangThaiGuiHoaDon
    {
        [Description("Chưa gửi hóa đơn cho khách hàng")]
        ChuaGui = 1,
        [Description("Đang gửi hóa đơn cho khách hàng")]
        DangGui = 2,
        [Description("Gửi hóa đơn cho khách hàng lỗi")]
        GuiLoi = 3,
        [Description("Đã gửi hóa đơn cho khách hàng")]
        DaGui = 4,
        [Description("Tất cả")]
        TatCa = 0
    }

    public class HoaDonThayTheSearch
    {
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
