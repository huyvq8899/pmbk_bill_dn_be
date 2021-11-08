using System.ComponentModel;

namespace DLL.Enums
{
    public enum TrangThaiSuDung
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chưa xác nhận")]
        ChuaXacThuc = 0,
        [Description("Đã xác thực")]
        DaXacThuc = 1,
        [Description("Đang sử dụng")]
        DangSuDung = 2,
        [Description("Ngừng sử dụng")]
        NgungSuDung = 3,
        [Description("Hết hiệu lực")]
        HetHieuLuc = 4
    }
}
