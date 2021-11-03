using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucHoaDon
    {
        [Description("Có mã của cơ quan thuế")]
        CoMa = 1,
        [Description("Không có mã của cơ quan thuế")]
        KhongCoMa = 0
    }
}
