using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TCHDon
    {
        [Description("Thay thế")]
        [XmlEnum("1")]
        ThayThe = 1,
        [Description("Điều chỉnh")]
        [XmlEnum("2")]
        DieuChinh = 2
    }
}
