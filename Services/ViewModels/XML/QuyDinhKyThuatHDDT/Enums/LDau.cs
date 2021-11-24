using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LDau
    {
        /// <summary>
        /// Lần đầu
        /// </summary>
        [Description("Lần đầu")]
        [XmlEnum("1")]
        LanDau = 1,
        /// <summary>
        /// Bổ sung
        /// </summary>
        [Description("Bổ sung")]
        [XmlEnum("0")]
        BoSung = 0
    }
}
