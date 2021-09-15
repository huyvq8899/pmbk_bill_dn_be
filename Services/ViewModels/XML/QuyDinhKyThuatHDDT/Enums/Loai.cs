using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum Loai
    {
        [Description("Thông báo hủy/giải trình của NNT")]
        ThongBaoHuyGiaiTrinhNTT = 1,
        [Description("Thông báo hủy/giải trình của NNT theo thông báo của CQT")]
        ThongBaoHuyGiaiTrinhNTTTheoThongBaoCQT = 2
    }
}
