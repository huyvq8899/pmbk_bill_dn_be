using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiHoaDon
    {
        [Description("Tất cả")]
        TatCa = -1,
        None,
        [Description("Hóa đơn GTGT")]
        HoaDonGTGT = 1,
        [Description("Hóa đơn bán hàng")]
        HoaDonBanHang = 2,
        [Description("Hóa đơn bán tài sản công")]
        HoaDonBanTaiSanCong = 3,
        [Description("Hóa đơn bán hàng dự trữ quốc gia")]
        HoaDonBanHangDuTruQuocGia = 4,
        [Description("Các loại hóa đơn khác")]
        CacLoaiHoaDonKhac = 5,
        [Description("Các chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn")]
        CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD = 6,
    }
}
