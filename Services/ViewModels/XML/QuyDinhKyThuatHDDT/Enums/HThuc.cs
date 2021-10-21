using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum HThuc
    {
        None = 0,
        /// <summary>
        /// Đăng ký mới
        /// </summary>
        [Description("Đăng ký mới")]
        DangKyMoi = 1,
        /// <summary>
        /// Thay đổi thông tin
        /// </summary>
         [Description("Thay đổi thông tin")]
        ThayDoiThongTin = 2
    }

    public enum HThuc2
    {
        /// <summary>
        /// Thêm mới
        /// </summary>
        ThemMoi = 1,
        /// <summary>
        /// Gia hạn
        /// </summary>
        GiaHan = 2,
        /// <summary>
        /// Ngừng sử dụng
        /// </summary>
        NgungSuDung = 3
    }
}
