﻿using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiQuyTrinh
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa ký điện tử")]
        ChuaKyDienTu,
        [Description("Đang ký điện tử")]
        DangKyDienTu,
        [Description("Ký điện tử lỗi")]
        KyDienTuLoi,
        [Description("Đã ký điện tử")]
        DaKyDienTu,
        [Description("Xóa ký điện tử")]
        XoaKyDienTu, // có mã cqt
        [Description("Chờ phản hồi")]
        ChoPhanHoi,
        [Description("Gửi CQT có lỗi")]
        GuiLoi,
        [Description("Gửi CQT không lỗi")]
        GuiKhongLoi,
        [Description("Không đủ điều kiện cấp mã")]
        KhongDuDieuKienCapMa, // có mã cqt
        [Description("CQT đã cấp mã")]
        CQTDaCapMa, // có mã cqt
        [Description("Hóa đơn không hợp lệ")]
        HoaDonKhongHopKe // không có mã cqt
    }
}
