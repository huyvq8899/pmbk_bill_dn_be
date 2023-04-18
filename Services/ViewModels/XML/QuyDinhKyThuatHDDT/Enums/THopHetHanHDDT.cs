using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum THopHetHanHDDT
    {
        [Description("Hết thời gian sử dụng hóa đơn có mã miễn phí")]
        [XmlEnum("1")]
        TruongHop1 = 1,
        [Description("Không còn thuộc trường hợp sử dụng hóa đơn điện tử không có mã")]
        [XmlEnum("2")]
        TruongHop2 = 2,
    }
}
