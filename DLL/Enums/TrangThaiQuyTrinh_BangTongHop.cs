using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLL.Enums
{
    public enum TrangThaiQuyTrinh_BangTongHop
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa gửi")]
        ChuaGui,
        [Description("Gửi TCTN lỗi")]
        GuiTCTNLoi,
        [Description("Chờ phản hồi")]
        ChoPhanHoi,
        [Description("Gửi CQT có lỗi")]
        GuiLoi,
        [Description("Gửi CQT không lỗi")]
        GuiKhongLoi,
        [Description("Bảng tổng hợp không hợp lệ")]
        BangTongHopKhongHopLe, // không có mã cqt
        [Description("Bảng tổng hợp hợp lệ")]
        BangTongHopHopLe, // không có mã cqt
        [Description("Bảng tổng hợp có hóa đơn không hợp lệ")]
        BangTongHopCoHoaDonKhongHopLe
    }
}
