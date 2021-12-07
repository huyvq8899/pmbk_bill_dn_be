using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiChietKhau
    {
        [Description("Không có chiết khấu")]
        KhongCoChietKhau,
        [Description("Theo mặt hàng")]
        TheoMatHang,
        [Description("Theo tổng giá trị hóa đơn")]
        TheoTongGiaTriHoaDon
    }
}
