using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TTXNCQT
    {
        [Description("Chấp nhận cho phép NNT sử dụng hóa đơn điện tử.")]
        [XmlEnum("1")]
        ChapNhan = 1,
        [Description("Không chấp nhận cho phép NNT sử dụng hóa đơn điện tử.")]
        [XmlEnum("2")]
        KhongChapNhan = 2
    }

    public enum TTXNCQT1
    {
        [Description("CQT chấp nhận")]
        [XmlEnum("1")]
        ChapNhan = 1,
        [Description("CQT không chấp nhận.")]
        [XmlEnum("2")]
        KhongChapNhan = 2
    }
}
