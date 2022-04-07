using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TChat
    {
        [XmlEnum("1")]
        [Description("Hàng hóa, dịch vụ")]
        HangHoaDichVu = 1,
        [XmlEnum("2")]
        [Description("Khuyến mại")]
        KhuyenMai = 2,
        [XmlEnum("3")]
        [Description("Chiết khấu thương mại")]
        ChietKhauThuongMai = 3,
        [XmlEnum("4")]
        [Description("Ghi chú, diễn giải")]
        GhiChuDienGiai = 4
    }
}
