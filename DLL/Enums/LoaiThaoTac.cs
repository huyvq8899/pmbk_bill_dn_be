using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLL.Enums
{
    public enum LoaiThaoTac
    {
        [Description("Tạo hóa đơn")]
        LapHoaDon = 0,
        [Description("Sửa hóa đơn")]
        SuaHoaDon = 1,
        [Description("Phát hành hóa đơn")]
        PhatHanhHoaDon = 2,
        [Description("Phát hành hóa đơn hàng loạt")]
        PhatHanhHoaDonHangLoat = 3,
        [Description("Gửi hóa đơn")]
        GuiHoaDon = 4,
        [Description("Gửi hóa đơn nháp")]
        GuiHoaDonNhap = 5,
        [Description("Gửi hóa đơn hàng loạt")]
        GuiHoaDonHangLoat = 6,
        [Description("Gửi hóa đơn nháp hàng loạt")]
        GuiHoaDonNhapHangLoat = 7,
        [Description("Xóa hóa đơn")]
        XoaHoaDon = 8,
        [Description("Xóa bỏ hóa đơn")]
        XoaBoHoaDon = 9,
        [Description("Chuyển thành hóa đơn giấy")]
        ChuyenThanhHoaDonGiay = 10,
        [Description("Chuyển thành hóa đơn giấy hàng loạt")]
        ChuyenThanhHoaDonGiayHangLoat = 11,
        [Description("Tải hóa đơn")]
        TaiHoaDon = 12,
        [Description("Xem hóa đơn")]
        XemHoaDon = 13,
        [Description("Nhập khẩu hóa đơn")]
        NhapKhauHoaDon = 14,
        [Description("Xuất khẩu hóa đơn")]
        XuatKhau = 15,
        [Description("Xuất khẩu chi tiết hóa đơn")]
        XuatKhauChiTietHoaDon = 16
    }
}
