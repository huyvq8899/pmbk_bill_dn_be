using System.ComponentModel;

namespace Services.Enums
{
    public enum BusinessOfType : int
    {
        NONE = 0,

        [Description("Phiếu thu")]
        PHIEU_THU = 1,

        [Description("Phiếu chi")]
        PHIEU_CHI = 2,

        [Description("Phiếu nhập")] // nhập kho khác
        PHIEU_NHAP = 3,

        [Description("Phiếu xuất")] // xuất kho khác
        PHIEU_XUAT = 4,

        [Description("Chứng từ bán hàng")]
        CHUNG_TU_BAN_HANG = 5,

        [Description("Chứng từ mua hàng")]
        CHUNG_TU_MUA_HANG = 6,

        [Description("Phiếu phân bổ dụng cụ")]
        PHIEU_TU_PBCP = 7,

        [Description("Chứng từ trả lại hàng mua")]
        CHUNG_TU_TRA_LAI_HANG_MUA = 8,

        [Description("Chứng từ trả lại hàng bán")]
        CHUNG_TU_TRA_LAI_HANG_BAN = 9,

        [Description("Phân bổ chi phí")]
        PHAN_BO_CHI_PHI = 10,

        [Description("Chứng từ nghiệp vụ khác")]
        CHUNG_TU_NGHIEP_VU_KHAC = 11,

        [Description("Tinh khấu hao TSCD")]
        TINH_KHAU_HAO_TSCD = 12,

        [Description("Đánh giá lại TSCD")]
        DANH_GIA_LAI_TSCD = 13,

        [Description("Ghi Giảm TSCD")]
        Ghi_GIAM_TSCD = 14,

        [Description("Phiếu chuyển kho")]
        PHIEU_CHUYEN_KHO = 15,

        [Description("Chứng từ giảm giá hàng mua")]
        CHUNG_TU_GIAM_GIA_HANG_MUA = 16,

        [Description("Chứng từ giảm giá hàng bán")]
        CHUNG_TU_GIAM_GIA_HANG_BAN = 17,

        [Description("Nhập kho đầu kỳ")]    // đầu kỳ
        KHO_NHAP_DAU_KY = 18,

        [Description("Nhập kho thành phẩm sản xuất")]   // nhập kho sản xuất
        PHIEU_NHAP_THANH_PHAM_SAN_XUAT = 19,

        [Description("Xuất kho NVL sản xuất")]  // xuất kho sản xuất
        PHIEU_XUAT_NVL_SAN_XUAT = 20,

        [Description("Lệnh sản xuất")]
        LENH_SAN_XUAT = 21,

        [Description("Nhập kho theo kiểm kê")]  // nhập kho kiểm kê
        PHIEU_NHAP_KIEM_KE = 22,

        [Description("Xuất kho theo kiểm kê")]  // xuất kho kiểm kê
        PHIEU_XUAT_KIEM_KE = 23,

        [Description("Kiểm kê kho")]
        KIEM_KE_KHO = 24,

        [Description("Kiểm kê tiền mặt")]
        KIEM_KE_TIEN_MAT = 25,

        [Description("Nhập kho chuyển kho")]    // chuyển kho
        KHO_NHAP_CHUYEN_KHO = 26,

        [Description("Xuất kho chuyển kho")]    // chuyển kho
        KHO_XUAT_CHUYEN_KHO = 27,

        [Description("Phân bổ chi phí ccdc")]
        PHAN_BO_CHI_PHI_CCDC = 28,

        [Description("Chuyển tài sản thuê tài chính thành tài sản sở hữu")]
        CHUYEN_TSCD_THUE_TAI_CHINH_THANH_SO_HUU = 29,

        [Description("Nhận hóa đơn")]
        NHAN_HOA_DON = 30,

        [Description("Nhập kho hàng nhận gia công")]    // nhập kho hàng gia công (ko ghi sổ)
        PHIEU_NHAP_HH_GIA_CONG = 31,

        [Description("Nhập kho hàng bán bị trả lại")]   // nhập kho hàng bán bị trả lại
        PHIEU_NHAP_TLHB = 32,

        [Description("Xuất kho bán hàng")]   // xuất kho bán hàng
        PHIEU_XUAT_BAN_HANG = 33,

        [Description("Phân bổ doanh thu nhận trước")]   // xuất kho bán hàng
        PHAN_BO_DOANH_THU_NHAN_TRUOC = 34,

        [Description("Ghi tăng tài sản cố định")]
        GHI_TANG_TSCD = 35,

        [Description("Điều chuyển tài sản cố định")]
        DIEU_CHUYEN_TSCD = 36,

        [Description("Kiểm kê tài sản cố định")]
        KIEM_KE_TSCD = 37,

        [Description("Kiểm kê chi phí trả trước")]
        KIEM_KE_CPTT = 38,

        [Description("Ghi giảm chi phí trả trước")]
        GHI_GIAM_CPTT = 39,

        [Description("Điều chuyển chi phí trả trước")]
        DIEU_CHUYEN_CPTT = 40,

        [Description("Điều chỉnh chi phí trả trước")]
        DIEU_CHINH_CPTT = 41,

        [Description("Ghi tăng chi phí trả trước")]
        GHI_TANG_CPTT = 42,

        [Description("Đơn mua hàng")]
        DON_MUA_HANG = 43,

        [Description("Hợp đồng mua")]
        HOP_DONG_MUA = 44,

        [Description("Hợp đồng bán")]
        HOP_DONG_BAN = 45,

        [Description("Báo giá")]
        BAO_GIA = 46,

        [Description("Đơn đặt hàng")]
        DON_DAT_HANG = 47,

        [Description("Xuất hóa đơn")]   // Xuất hóa đơn
        XUAT_HOA_DON = 48,

        [Description("Doanh thu nhận trước")]   // xuất kho bán hàng
        DOANH_THU_NHAN_TRUOC = 49,

        [Description("Điều chỉnh tài sản cố định")]   // xuất kho bán hàng
        DIEU_CHINH_TSCD = 50,

        [Description("Hóa đơn trả lại hàng bán")]
        HOA_DON_TRA_LAI_HANG_BAN = 51,

        [Description("Hóa đơn giảm giá hàng bán")]
        HOA_DON_GIAM_GIA_HANG_BAN = 52,

        [Description("Hóa đơn giá trị gia tăng")]
        HOA_DON_GIA_TRI_GIA_TANG = 53,

        [Description("Hóa đơn bán hàng")]
        HOA_DON_BAN_HANG = 54
    }
}
