using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
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
