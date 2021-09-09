using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum THop
    {
        [Description("Trường hợp tiếp nhận Tờ khai đăng ký sử dụng hóa đơn điện tử")]
        TruongHop1 = 1,
        [Description("Trường hợp không tiếp nhận Tờ khai đăng ký sử dụng hóa đơn điện tử")]
        TruongHop2 = 2,
        [Description("Trường hợp tiếp nhận Tờ khai đăng ký thay đổi thông tin sử dụng hóa đơn điện tử")]
        TruongHop3 = 3,
        [Description("Trường hợp không tiếp nhận Tờ khai đăng ký thay đổi thông tin sử dụng hóa đơn điện tử")]
        TruongHop4 = 4
    }
}
