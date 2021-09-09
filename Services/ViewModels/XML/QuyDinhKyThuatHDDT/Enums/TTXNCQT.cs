using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum TTXNCQT
    {
        [Description("Trường hợp chấp nhận cho phép NNT sử dụng hóa đơn điện tử")]
        ChapNhan = 1,
        [Description("Trường hợp không chấp nhận cho phép NNT sử dụng hóa đơn điện tử")]
        KhongChapNhan = 2
    }
}
