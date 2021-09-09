using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum HThuc
    {
        [Description("Đăng ký mới")]
        DangKyMoi = 1,
        [Description("Thay đổi thông tin")]
        ThayDoiThongTin = 2
    }

    public enum HThuc2
    {
        [Description("Thêm mới")]
        ThemMoi = 1,
        [Description("Gia hạn")]
        GiaHan = 2,
        [Description("Ngừng sử dụng")]
        NgungSuDung = 3
    }
}
