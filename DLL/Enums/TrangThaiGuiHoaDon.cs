using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiGuiHoaDon
    {
        [Description("Chưa gửi cho khách hàng")]
        ChuaGui,
        [Description("Đang gửi cho khách hàng")]
        DangGui,
        [Description("Gửi cho khách hàng lỗi")]
        GuiLoi,
        [Description("Đã gửi cho khách hàng")]
        DaGui,
        [Description("Khách hàng đã xem")]
        KhachHangDaXem,
        [Description("Khách hàng chưa xem")]
        KhachHangChuaXem
    }
}
