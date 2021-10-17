using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLL.Enums
{
    public enum TrangThaiGuiToKhaiDenCQT
    {
        [Description("Chưa gửi")]
        ChuaGui = 0,
        [Description("Đã gửi")]
        DaGui = 1,
        [Description("Đã tiếp nhận")]
        DaTiepNhan = 2,
        [Description("Từ chối tiếp nhận")]
        TuChoiTiepNhan = 2,
    }

    public enum TrangThaiTiepNhanCuaCoQuanThue
    {
        [Description("Chưa phản hồi")]
        ChuaPhanHoi = 0,
        [Description("Chấp nhận")]
        ChapNhan = 1,
        [Description("Không chấp nhận")]
        KhongChapNhan = 2,
    }
}
