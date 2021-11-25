using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLL.Enums
{
    public enum TrangThaiGuiToKhaiDenCQT
    {
        [Description("Chưa gửi")]
        ChuaGui = -1,
        [Description("Chờ phản hồi")]
        ChoPhanHoi = 0,
        [Description("CQT tiếp nhận")]
        DaTiepNhan = 1,
        [Description("CQT không tiếp nhận")]
        TuChoiTiepNhan = 2,
        [Description("Gửi CQT không lỗi")]
        GuiKhongLoi = 3,
        [Description("Gửi CQT có lỗi")]
        GuiLoi = 4,
        [Description("CQT chấp nhận")]
        ChapNhan = 5,
        [Description("CQT không chấp nhận")]
        KhongChapNhan = 6,
    }
}
