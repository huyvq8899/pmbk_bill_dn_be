using System;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LHHoa
    {
        [XmlEnum("1")]
        XangDau = 1,
        [XmlEnum("2")]
        VanTaiHangKhong = 2,
        [XmlEnum("9")]
        Khac = 9,
    }
}
