using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum TTTNhan
    {
        /// <summary>
        /// Không lỗi
        /// </summary>
        [Description("Không lỗi")]
        [XmlEnum("0")]
        KhongLoi,
        /// <summary>
        /// Có lỗi
        /// </summary>
        [Description("Có lỗi")]
        [XmlEnum("1")]
        CoLoi
    }

    public enum TTTNhan1
    {
        /// <summary>
        /// Không lỗi
        /// </summary>
        [Description("Gửi CQT không lỗi")]
        [XmlEnum("0")]
        KhongLoi,
        /// <summary>
        /// Có lỗi
        /// </summary>
        [Description("Gửi CQT có lỗi")]
        [XmlEnum("1")]
        CoLoi
    }
}
