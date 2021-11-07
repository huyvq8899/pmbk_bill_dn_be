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
        [XmlEnum("1")]
        CoLoi
    }
}
