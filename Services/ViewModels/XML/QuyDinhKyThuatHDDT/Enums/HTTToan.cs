using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HTTToan
    {
        /// <summary>
        /// Tiền mặt
        /// </summary>
        [Description("Tiền mặt")]
        [XmlEnum("1")]
        TienMat = 1,
        /// <summary>
        /// Chuyển khoản
        /// </summary>
        [Description("Chuyển khoản")]
        [XmlEnum("2")]
        ChuyenKhoan = 2,
        /// <summary>
        /// Tiền mặt/Chuyển khoản
        /// </summary>
        [Description("Tiền mặt/Chuyển khoản")]
        [XmlEnum("3")]
        TienMatChuyenKhoan = 3,
        /// <summary>
        /// Đối trừ công nợ
        /// </summary>
        [Description("Đối trừ công nợ")]
        [XmlEnum("4")]
        DoiTruCongNo = 4,
        /// <summary>
        /// Không thu tiền
        /// </summary>
        [Description("Không thu tiền")]
        [XmlEnum("5")]
        KhongThuTien = 5,
        /// <summary>
        /// Khác
        /// </summary>
        [Description("Khác")]
        [XmlEnum("9")]
        Khac = 9
    }
}
