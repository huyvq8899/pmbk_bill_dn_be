using System.ComponentModel;

namespace DLL.Enums
{
    public enum UyNhiemLapHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Đăng ký")]
        DangKy = 1,
        [Description("Không đăng ký")]
        KhongDangKy = 0
    }
}
