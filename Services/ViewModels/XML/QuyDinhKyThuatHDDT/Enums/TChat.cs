using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum TChat
    {
        [Description("Hàng hóa, dịch vụ")]
        HangHoaDichVu = 1,
        [Description("Khuyến mại")]
        KhuyenMai = 2,
        [Description("Chiết khấu thương mại (trong trường hợp muốn thể hiện thông tin chiết khấu theo dòng)")]
        ChietKhauThuongMai = 3,
        [Description("Ghi chú, diễn giải")]
        GhiChuDienGiai = 4
    }
}
