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
        HinhNenTaiLen,
        [Description("Khung viền mặc định")]
        KhungVienMacDinh,
        [Description("Tên dịch vụ")]
        TenDichVu,
        [Description("Chiều cao giữa các dòng")]
        ChieuCaoGiuaCacDong,
        [Description("Hình thức hiển thị số vé")]
        HinhThucHienThiSoVe,
        [Description("Tổng tiền")]
        TongTien,
        [Description("Thuế GTGT")]
        ThueGTGT,
        [Description("Vị trí hiển thị tổng tiền")]
        ViTriHienThiTongTien,
        [Description("Hiển thị đơn vị tiền tệ sau số tiền")]
        HienThiDonViTienTeSauSoTien,
        [Description("Đơn vị tiền tệ")]
        DonViTienTe,
        [Description("Hiển thị QR Code tại vị trí")]
        HienThiQRCodeTaiViTri,
        [Description("Vị trí QR Code")]
        ViTriQRCode
    }
}
