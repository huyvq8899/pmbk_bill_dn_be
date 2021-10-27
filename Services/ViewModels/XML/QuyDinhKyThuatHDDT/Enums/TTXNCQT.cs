using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TTXNCQT
    {
        [Description("Trường hợp chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        [XmlEnum("1")]
        ChapNhan = 1,
        [Description("Trường hợp không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        [XmlEnum("2")]
        KhongChapNhan = 2
    }
}
