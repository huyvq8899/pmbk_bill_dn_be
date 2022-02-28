using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucMauHoaDon
    {
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauCoBan,
        [Description("(Hóa đơn chuyển đổi từ hóa đơn điện tử)")]
        HoaDonMauDangChuyenDoi,
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauCoChietKhau,
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauNgoaiTe,
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauCoBan_CoChietKhau,
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauCoBan_NgoaiTe,
        [Description("(Bản thể hiện của hóa đơn điện tử)")]
        HoaDonMauCoBan_All,
        [Description("(Hóa đơn chuyển đổi từ hóa đơn điện tử)")]
        HoaDonMauDangChuyenDoi_CoChietKhau,
        [Description("(Hóa đơn chuyển đổi từ hóa đơn điện tử)")]
        HoaDonMauDangChuyenDoi_NgoaiTe,
        [Description("(Hóa đơn chuyển đổi từ hóa đơn điện tử)")]
        HoaDonMauDangChuyenDoi_All,
    }
}
