using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiHoaDon
    {
        [Description("Hóa đơn giá trị gia tăng")]
        HoaDonGTGT = 1,
        [Description("Hóa đơn bán hàng")]
        HoaDonBanHang = 2,
        [Description("Hóa đơn bán hàng (dành cho tổ chức, cá nhân trong khu phi thuế quan)")]
        HoaDonBanHangDanhChoTCCN = 3,
        [Description("PXK kiêm vận chuyển nội bộ")]
        PXKKiemVanChuyenNoiBo = 4,
        [Description("PXK hàng gửi bán đại lý")]
        PXKHangGuiBanDaiLy = 5,

    }
}
