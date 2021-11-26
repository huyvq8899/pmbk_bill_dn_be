using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiGuiThongDiep
    {
        [Description("Chưa gửi")]
        ChuaGui = -1,
        [Description("Chờ phản hồi")]
        ChoPhanHoi = 0,
        [Description("Gửi CQT không lỗi")]
        GuiKhongLoi = 3,
        [Description("Gửi CQT có lỗi")]
        GuiLoi = 4,
        [Description("CQT tiếp nhận")]
        DaTiepNhan = 1,
        [Description("CQT không tiếp nhận")]
        TuChoiTiepNhan = 2,
        [Description("CQT chấp nhận")]
        ChapNhan = 5,
        [Description("CQT không chấp nhận")]
        KhongChapNhan = 6,
        [Description("Không đủ điều kiện cấp mã")]
        KhongDuDieuKienCapMa = 7,
        [Description("CQT đã cấp mã")]
        CQTDaCapMa = 8,
    }
}
