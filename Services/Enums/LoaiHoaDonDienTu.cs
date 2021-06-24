using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum LoaiHoaDonDienTu
    {
        [Description("Hóa đơn giá trị gia tăng")]
        HOA_DON_GIA_TRI_GIA_TANG = 1,

        [Description("Hóa đơn bán hàng")]
        HOA_DON_BAN_HANG = 2,
    }
}
