using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiGuiEmail
    {
        [Description("Gửi cho khách hàng bị lỗi")]
        GuiLoi,
        [Description("Đã gửi cho khách hàng")]
        DaGui
    }

    public enum TrangThaiGuiEmailV2
    {
        [Description("Gửi cho khách hàng bị lỗi")]
        GuiLoi,
        [Description("Đã gửi cho khách hàng")]
        DaGui,
        [Description("Đang gửi cho khách hàng")]
        DangGuiChoKhachHang,
        [Description("Khách hàng đã nhận")]
        KhachHangDaNhan
    }
}
