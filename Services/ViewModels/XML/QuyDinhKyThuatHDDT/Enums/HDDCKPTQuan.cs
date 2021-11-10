using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HDDCKPTQuan
    {
        /// <summary>
        /// Hóa đơn không dành cho tổ chức, cá nhân trong khu phi thuế quan
        /// </summary>
        [Description("Hóa đơn không dành cho tổ chức, cá nhân trong khu phi thuế quan")]
        [XmlEnum("0")]
        HoaDonKhongDanhChoToChucTrongKhuPhiThueQuan,
        /// <summary>
        /// Hóa đơn dành cho tổ chức, cá nhân trong khu phi thuế quan,
        /// </summary>
        [Description("Hóa đơn dành cho tổ chức, cá nhân trong khu phi thuế quan")]
        [XmlEnum("1")]
        HoaDonDanhChoToChucTrongKhuPhiThueQuan
    }
}
