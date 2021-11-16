using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiGuiHoaDon
    {
        [Description("Chưa gửi hóa đơn cho khách hàng")]
        ChuaGui,
        [Description("Đang gửi hóa đơn cho khách hàng")]
        DangGui,
        [Description("Gửi hóa đơn cho khách hàng lỗi")]
        GuiLoi,
        [Description("Đã gửi hóa đơn cho khách hàng")]
        DaGui,
        [Description("Khách hàng đã xem")]
        KhachHangDaXem,
        [Description("Khách hàng chưa xem")]
        KhachHangChuaXem
    }
}
