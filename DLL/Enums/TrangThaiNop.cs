using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiNop
    {
        [Description("Chưa nộp cho CQ thuế")]
        ChuaNop = 0,
        [Description("Đã nộp cho CQ thuế")]
        DaNop = 1,
        [Description("Đã có hiệu lực")]
        DaDuocChapNhan = 2
    }
}
