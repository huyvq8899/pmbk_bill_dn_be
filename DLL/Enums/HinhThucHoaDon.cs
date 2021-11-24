using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Có mã của CQT")]
        CoMa = 1,
        [Description("Không có mã của CQT")]
        KhongCoMa = 0
    }
}
