using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum TrangThaiPhatHanh
    {
        [Description("Chưa phát hành")]
        ChuaPhatHanh,
        [Description("Đang phát hành")]
        DangPhatHanh,
        [Description("Phát hành lỗi")]
        PhatHanhLoi,
        [Description("Đã phát hành")]
        DaPhatHanh
    }
}
