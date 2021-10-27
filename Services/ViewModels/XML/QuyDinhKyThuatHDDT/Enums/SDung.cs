using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum SDung
    {
        [Description("Không sử dụng")]
        [XmlEnum("0")]
        KhongSuDung,
        [Description("Sử dụng")]
        [XmlEnum("1")]
        SuDung,
    }
}
