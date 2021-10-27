using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HDXKPTQuan
    {
        /// <summary>
        /// Không phải Hóa đơn xuất vào khu phi thuế quan
        /// </summary>
        [Description("Không phải Hóa đơn xuất vào khu phi thuế quan")]
        [XmlEnum("0")]
        KhongPhaiHoaDonXuatVaoKhuPhiThueQuan,
        /// <summary>
        /// Hóa đơn xuất vào khu phi thuế quan
        /// </summary>
        [Description("Hóa đơn xuất vào khu phi thuế quan")]
        [XmlEnum("1")]
        HoaDonXuatVaoKhuPhiThueQuan
    }
}
