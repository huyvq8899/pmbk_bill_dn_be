using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum HThuc
    {
        None = 0,
        /// <summary>
        /// Đăng ký mới
        /// </summary>
        [Description("Đăng ký mới")]
        [XmlEnum("1")]
        DangKyMoi = 1,
        /// <summary>
        /// Thay đổi thông tin
        /// </summary>
        [Description("Thay đổi thông tin")]
        [XmlEnum("2")]
        ThayDoiThongTin = 2
    }

    [Serializable]
    public enum HThuc2
    {
        /// <summary>
        /// Thêm mới
        /// </summary>
        [Description("Thêm mới")]
        [XmlEnum("1")]
        ThemMoi = 1,
        /// <summary>
        /// Gia hạn
        /// </summary>
        [Description("Gia hạn")]
        [XmlEnum("2")]
        GiaHan = 2,
        /// <summary>
        /// Ngừng sử dụng
        /// </summary>
        [Description("Ngừng sử dụng")]
        [XmlEnum("3")]
        NgungSuDung = 3
    }
}
