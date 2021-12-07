using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucXoabo
    {
        [Description("Hủy hóa đơn để lập hóa đơn gốc mới")]
        HinhThuc1 = 1,
        [Description("Xóa hóa đơn để lập hóa đơn thay thế")]
        HinhThuc2 = 2,
        [Description("Hủy hóa đơn do hợp đồng mua bán bị hủy")]
        HinhThuc3 = 3,
        [Description("Hủy hóa đơn để lập hóa đơn thay thế mới")]
        HinhThuc4 = 4,
        [Description("Xóa hóa đơn để lập hóa đơn thay thế khác")]
        HinhThuc5 = 5,
        [Description("Hủy hóa đơn để lập hóa đơn điều chỉnh mới")]
        HinhThuc6 = 6,
    }
}
