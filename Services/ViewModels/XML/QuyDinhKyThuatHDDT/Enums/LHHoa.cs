using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LHHoa
    {
        [XmlEnum("1")]
        [Description("Xăng dầu")]
        XangDau = 1,
        [XmlEnum("2")]
        [Description("Vận tải - hàng không")]
        VanTaiHangKhong = 2,
        [XmlEnum("9")]
        [Description("Khác")]
        Khac = 9,
    }
}
