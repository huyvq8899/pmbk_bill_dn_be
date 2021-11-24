using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum KQKTra
    {
        /// <summary>
        /// Không hợp lệ
        /// </summary>
        [Description("Không hợp lệ")]
        [XmlEnum("0")]
        KhongHopLe,
        /// <summary>
        /// Hợp lệ
        /// </summary>
        [Description("Hợp lệ")]
        [XmlEnum("1")]
        HopLe
    }
}
