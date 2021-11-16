using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiQuyTrinh
    {
        [Description("Chưa phát hành")]
        ChuaPhatHanh,
        [Description("Đang phát hành")]
        DangPhatHanh,
        [Description("Phát hành lỗi")]
        PhatHanhLoi,
        [Description("Đã phát hành")]
        DaPhatHanh,
        [Description("Đang ký điện tử")]
        DangKyDienTu,
        [Description("Ký điện tử lỗi")]
        KyDienTuLoi,
        [Description("Đã ký điện tử")]
        DaKyDienTu,
        [Description("Chờ phản hồi")]
        ChoPhanHoi,
        [Description("Gửi lỗi")]
        GuiLoi,
        [Description("Gửi không lỗi")]
        GuiKhongLoi,
        [Description("Hóa đơn không hợp lệ")]
        HoaDonKhongHopLe, // không có mã cqt
        [Description("Không đủ điều kiện cấp mã")]
        KhongDuDieuKienCapMa, // có mã cqt
        [Description("CQT đã cấp mã")]
        CQTDaCapMa, // có mã cqt
    }
}
