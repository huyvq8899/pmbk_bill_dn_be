using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLL.Enums
{
    public enum TrangThaiGuiToKhaiDenCQT
    {
        [Description("Đã tiếp nhận")]
        DaTiepNhan = 1,
        [Description("Từ chối tiếp nhận")]
        TuChoiTiepNhan = 2,
    }

    public enum TrangThaiTiepNhanCuaCoQuanThue
    {
        [Description("Chấp nhận")]
        ChapNhan = 1,
        [Description("Không chấp nhận")]
        KhongChapNhan = 2,
    }
}
