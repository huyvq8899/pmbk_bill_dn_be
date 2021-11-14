using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum HinhThucThanhToan
    {
        [Description("Tiền mặt")]
        TienMat = 1,
        [Description("Chuyển khoản")]
        CK = 2,
        [Description("Tiền mặt/Chuyển khoản")]
        TMCK = 3,
        [Description("Đối trừ công nợ")]
        DoiTruCongNo = 4,
        [Description("Không thu tiền")]
        KhongThuTien = 5,
        [Description("Khác")]
        Khac = 6,
    }
}
