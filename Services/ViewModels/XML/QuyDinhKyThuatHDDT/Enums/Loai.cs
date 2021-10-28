using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum Loai
    {
        [Description("Thông báo hủy/giải trình của NNT")]
        [XmlEnum("1")]
        ThongBaoHuyGiaiTrinhNTT = 1,
        [Description("Thông báo hủy/giải trình của NNT theo thông báo của CQT")]
        [XmlEnum("2")]
        ThongBaoHuyGiaiTrinhNTTTheoThongBaoCQT = 2
    }
}
