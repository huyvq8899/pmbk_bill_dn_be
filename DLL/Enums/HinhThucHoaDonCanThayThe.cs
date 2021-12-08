using System.ComponentModel;

namespace DLL.Enums
{
    public enum HinhThucHoaDonCanThayThe
    {
        [Description("Hóa đơn điện tử theo Nghị định 123/2020/NĐ-CP")]
        HinhThuc1 = 1,

        [Description("Hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC ngày 23 tháng 6 năm 2015 và Quyết định số 2660/QĐ-BTC ngày 14 tháng 12 năm 2016 của Bộ Tài chính (Hóa đơn có mã xác thực của CQT theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP)")]
        HinhThuc2 = 2,

        [Description("Các loại hóa đơn theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP (Trừ hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC và Quyết định số 2660/QĐ-BTC)")]
        HinhThuc3 = 3,

        [Description("Hóa đơn đặt in theo Nghị định 123/2020/NĐ-CP")]
        HinhThuc4 = 4
    }

    public enum TrangThaiHoaDonNgoaiHeThong
    {
        [Description("Hóa đơn gốc")]
        TrangThai1 = 1,

        [Description("Thay thế")]
        TrangThai2 = 3,

        [Description("Điều chỉnh")]
        TrangThai3 = 4
    }
}
