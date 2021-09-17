using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum HTTToan
    {
        /// <summary>
        /// Tiền mặt
        /// </summary>
        TienMat = 1,
        /// <summary>
        /// Chuyển khoản
        /// </summary>
        ChuyenKhoan = 2,
        /// <summary>
        /// Tiền mặt/Chuyển khoản
        /// </summary>
        TienMatChuyenKhoan = 3,
        /// <summary>
        /// Đối trừ công nợ
        /// </summary>
        DoiTruCongNo = 4,
        /// <summary>
        /// Không thu tiền
        /// </summary>
        KhongThuTien = 5,
        /// <summary>
        /// Khác
        /// </summary>
        Khac = 9
    }
}
