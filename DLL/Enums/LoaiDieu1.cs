using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiDieu1
    {
        [Description("Máy tính: ")]
        MayTinh = 1,
        [Description("Máy in: ")]
        MayIn = 2,
        [Description("Chứng thư in: ")]
        ChungThuSo = 3,
        [Description("Thiết bị: ")]
        ThietBi = 4,
        [Description("Phần mềm ứng dụng: ")]
        PhanMemUngDung = 5
    }
}
