using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum TrangThaiBienBanXoaBo
    {
        [Description("Chưa lập")]
        ChuaLap,
        [Description("Chưa ký")]
        ChuaKy,
        [Description("Chưa gửi khách hàng")]
        ChuaGuiKH,
        [Description("Chờ khách hàng ký")]
        ChoKHKy,
        [Description("Khách hàng đã ký")]
        KHDaKy
    }
}
