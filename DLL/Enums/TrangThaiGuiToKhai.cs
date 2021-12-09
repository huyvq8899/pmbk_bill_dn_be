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
        [Description("Có cặp ủy nhiệm CQT không chấp nhận")]
        CoUNCQTKhongChapNhan = 9,

        [Description("Có hóa đơn không hợp lệ")]
        CoHDKhongHopLe = 10,
        [Description("Gói dữ liệu không hợp lệ")]
        GoiDuLieuKhongHopLe = 11,
        [Description("Gói dữ liệu hợp lệ")]
        GoiDuLieuHopLe = 12,

        [Description("CQT tiếp nhận tất cả hóa đơn")]
        CQTTiepNhanTatCaHoaDon = 13,
        [Description("Có hóa đơn CQT không tiếp nhận")]
        CoHoaDonCQTKhongTiepNhan = 14
    }
}
