using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiThietLapMacDinh
    {
        [Description("Ảnh logo")]
        Logo,
        [Description("Kiểu chữ")]
        KieuChu,
        [Description("Cỡ chữ")]
        CoChu,
        [Description("Màu chữ")]
        MauChu,
        [Description("Hiển thị QR-code")]
        HienThiQRCode,
        [Description("Lặp lại thông tin khi hóa đơn có nhiều trang")]
        LapLaiThongTinKhiHoaDonCoNhieuTrang,
        [Description("Thiết lập dòng ký hiệu cột")]
        ThietLapDongKyHieuCot,
        [Description("Số dòng trắng")]
        SoDongTrang,
        [Description("Hình nền mặc định")]
        HinhNenMacDinh,
        [Description("Hình nền tải lên")]
        HinhNenTaiLen
    }
}
