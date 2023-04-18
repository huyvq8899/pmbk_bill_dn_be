using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum ADung
    {
        /// <summary>
        /// Không áp dụng
        /// </summary>
        [Description("Không áp dụng")]
        [XmlEnum("0")]
        KhongApDung,
        /// <summary>
        /// Áp dụng
        /// </summary>
        [Description("Áp dụng")]
        [XmlEnum("1")]
        ApDung
    }
}
