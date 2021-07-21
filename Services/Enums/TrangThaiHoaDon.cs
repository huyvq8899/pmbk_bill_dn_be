using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum TrangThaiHoaDon
    {
        [Description("Hóa đơn gốc")]
        HoaDonGoc = 1,
        [Description("Hóa đơn xóa bỏ")]
        HoaDonXoaBo = 2,
        [Description("Hóa đơn thay thế")]
        HoaDonThayThe = 3,
        [Description("Hóa đơn điều chỉnh")]
        HoaDonDieuChinh = 4
    }
}
