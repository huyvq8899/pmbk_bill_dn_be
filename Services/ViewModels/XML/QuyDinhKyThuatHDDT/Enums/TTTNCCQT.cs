using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TTTNCCQT
    {
        [Description("Tiếp nhận")]
        [XmlEnum("1")]
        TiepNhan = 1,
        [Description("Không tiếp nhận")]
        [XmlEnum("2")]
        KhongTiepNhan = 2
    }
}
