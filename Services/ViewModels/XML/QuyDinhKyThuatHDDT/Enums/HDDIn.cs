using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HDDIn
    {
        /// <summary>
        /// Hóa đơn in
        /// </summary>
        [Description("Hóa đơn in")]
        [XmlEnum("1")]
        HoaDonIn = 1,
        /// <summary>
        /// Hóa đơn điện tử
        /// </summary>
        [Description("Hóa đơn điện tử")]
        [XmlEnum("0")]
        HoaDonDienTu = 0
    }
}
