using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum THop
    {
        [Description("Tiếp nhận Tờ khai đăng ký sử dụng hóa đơn điện tử")]
        [XmlEnum("1")]
        TruongHop1 = 1,
        [Description("Không tiếp nhận Tờ khai đăng ký sử dụng hóa đơn điện tử")]
        [XmlEnum("2")]
        TruongHop2 = 2,
        [Description("Tiếp nhận Tờ khai đăng ký thay đổi thông tin sử dụng hóa đơn điện tử")]
        [XmlEnum("3")]
        TruongHop3 = 3,
        [Description("Không tiếp nhận Tờ khai đăng ký thay đổi thông tin sử dụng hóa đơn điện tử")]
        [XmlEnum("4")]
        TruongHop4 = 4
    }
}
