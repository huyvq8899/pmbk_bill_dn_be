using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucHoaDonCanThayThe
    {
        [Description("Hóa đơn tự in")]
        HoaDonTuIn = 1,
        [Description("Hóa đơn đặt in")]
        HoaDonDatIn = 2,
        [Description("Hóa đơn điện tử lập từ hệ thống khác")]
        HoaDonDienTuLapTuHeThongKhac = 3
    }
}
