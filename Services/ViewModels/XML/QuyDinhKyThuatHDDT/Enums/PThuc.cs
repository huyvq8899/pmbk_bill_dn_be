using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum PThuc
    {
        [Description("Tiền mặt")]
        TienMat = 1,
        [Description("Chuyển khoản")]
        ChuyenKhoan = 2,
        [Description("Tiền mặt/Chuyển khoản")]
        TienMatChuyenKhoan = 3,
        [Description("Đối trừ công nợ")]
        DoiTruCongNo = 4,
        [Description("Không thu tiền")]
        KhongThuTien = 5,
        [Description("Khác")]
        Khac = 9
    }
}
