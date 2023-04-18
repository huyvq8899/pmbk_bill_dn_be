using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum KQua
    {
        /// <summary>
        /// Chấp nhận
        /// </summary>
        [Description("Chấp nhận")]
        [XmlEnum("1")]
        ChapNhan = 1,
        /// <summary>
        /// Không chấp nhận
        /// </summary>
        [Description("Không chấp nhận")]
        [XmlEnum("2")]
        KhongChapNhan = 2
    }
}
