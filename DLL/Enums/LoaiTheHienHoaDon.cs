using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiTheHienHoaDon
    {
        [Description("Hóa đơn mẫu")]
        HoaDonMau,
        [Description("Hóa đơn dạng chuyển đổi")]
        HoaDonDangChuyenDoi,
        [Description("Hóa đơn có chiết khấu")]
        HoaDonCoChietKhau,
        [Description("Hóa đơn ngoại tệ")]
        HoaDonNgoaiTe
    }
}
