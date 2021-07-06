using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiThueGTGT
    {
        [Description("Không thuế suất")]
        KhongThueSuat = 0,
        [Description("Mẫu một thuế suất")]
        MauMotThueSuat = 1,
        [Description("Mẫu nhiều thuế suất")]
        MauNhieuThueSuat = 2
    }
}
