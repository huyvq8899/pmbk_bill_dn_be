using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LDKUNhiem
    {
        /// <summary>
        /// Ủy nhiệm
        /// </summary>
        [Description("Ủy nhiệm")]
        [XmlEnum("1")]
        UyNhiem = 1,
        /// <summary>
        /// Nhận ủy nhiệm
        /// </summary>
        [Description("Nhận ủy nhiệm")]
        [XmlEnum("2")]
        NhanUyNhiem = 2
    }
}
