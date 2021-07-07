using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiThietLapMacDinh
    {
        [Description("Ảnh logo")]
        Logo = 1,
        [Description("Kiểu chữ")]
        KieuChu = 2,
        [Description("Cỡ chữ")]
        CoChu = 3,
        [Description("Màu chữ")]
        MauChu = 4,
        [Description("Hiển thị QR-code")]
        HienThiQRCode = 5,
        [Description("Lặp lại thông tin khi hóa đơn có nhiều trang")]
        LapLaiThongTinKhiHoaDonCoNhieuTrang = 6,
        [Description("Thiết lập dòng ký hiệu cột")]
        ThietLapDongKyHieuCot = 7,
        [Description("Số dòng trắng")]
        SoDongTrang = 8
    }
}
