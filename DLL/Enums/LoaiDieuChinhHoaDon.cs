using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiDieuChinhHoaDon
    {
        [Description("Hóa đơn điều chỉnh tăng")]
        DieuChinhTang = 1,
        [Description("Hóa đơn điều chỉnh giảm")]
        DieuChinhGiam = 2,
        [Description("Hóa đơn điều chỉnh thông tin")]
        DieuChinhThongTin = 3
    }
}
