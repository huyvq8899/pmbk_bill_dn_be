using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum THopHetHanHDDT
    {
        [Description("Hết thời gian sử dụng hóa đơn có mã miễn phí")]
        TruongHop1 = 1,
        [Description("Không còn thuộc trường hợp sử dụng hóa đơn điện tử không có mã")]
        TruongHop2 = 2,
    }
}
