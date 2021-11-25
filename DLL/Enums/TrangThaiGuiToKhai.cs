using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiGuiThongDiep
    {
        [Description("Chưa gửi")]
        ChuaGui = -1,
        [Description("Chờ phản hồi")]
        ChoPhanHoi = 0,
        [Description("Đã tiếp nhận")]
        DaTiepNhan = 1,
        [Description("Từ chối tiếp nhận")]
        TuChoiTiepNhan = 2,
        [Description("Gửi không lỗi")]
        GuiKhongLoi = 3,
        [Description("Gửi lỗi")]
        GuiLoi = 4,
        [Description("Chấp nhận")]
        ChapNhan = 5,
        [Description("Không chấp nhận")]
        KhongChapNhan = 6,
        [Description("Không đủ điều kiện cấp mã")]
        KhongDuDieuKienCapMa = 7,
        [Description("CQT đã cấp mã")]
        CQTDaCapMa = 8,
    }
}
