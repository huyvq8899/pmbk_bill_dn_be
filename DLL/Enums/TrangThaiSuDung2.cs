using System.ComponentModel;

namespace DLL.Enums
{
    /// <summary>
    /// trạng thái sử dụng cho thông tin hóa đơn
    /// </summary>
    public enum TrangThaiSuDung2
    {
        [Description("Không sử dụng")]
        KhongSuDung,
        [Description("Đang sử dụng")]
        DangSuDung,
        [Description("Ngừng sử dụng")]
        NgungSuDung
    }
}
