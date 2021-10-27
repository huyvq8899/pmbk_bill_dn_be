using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TCTBao
    {
        [Description("Mới")]
        [XmlEnum("0")]
        TCTBao0 = 0,
        [Description("Hủy")]
        [XmlEnum("1")]
        TCTBao1 = 1,
        [Description("Điều chỉnh")]
        [XmlEnum("2")]
        TCTBao2 = 2,
        [Description("Thay thế")]
        [XmlEnum("3")]
        TCTBao3 = 3,
        [Description("Giải trình")]
        [XmlEnum("4")]
        TCTBao4 = 4,
        [Description("Sai sót do tổng hợp")]
        [XmlEnum("5")]
        TCTBao5 = 5,
    }
}
