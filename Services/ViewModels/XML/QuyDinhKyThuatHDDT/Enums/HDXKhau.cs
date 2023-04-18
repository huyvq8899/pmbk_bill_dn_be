using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HDXKhau
    {
        /// <summary>
        /// Không phải Hóa đơn xuất khẩu
        /// </summary>
        [Description("Không phải Hóa đơn xuất khẩu")]
        [XmlEnum("0")]
        KhongPhaiHoaDonXuatKhau,
        /// <summary>
        /// Hóa đơn xuất khẩu
        /// </summary>
        [Description("Hóa đơn xuất khẩu")]
        [XmlEnum("1")]
        HoaDonXuatKhau
    }
}
