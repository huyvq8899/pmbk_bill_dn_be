using System.ComponentModel;

namespace Services.Enums
{
    public enum LoaiHanhDong
    {
        [Description("Thêm")]
        Them,
        [Description("Sửa")]
        Sua,
        [Description("Xóa")]
        Xoa,
        [Description("Xuất khẩu")]
        XuatKhau,
        [Description("In")]
        In,
        [Description("Ký điện tử")]
        KyNhay,
        [Description("Phát hành hóa đơn thành công")]
        PhatHanhHoaDonThanhCong,
        [Description("Phát hành hóa đơn thất bại")]
        PhatHanhHoaDonThatBai,
        [Description("Chuyển thành hóa đơn giấy")]
        ChuyenThanhHoaDonGiay,
        [Description("Gửi HĐ cho KH")]
        GuiHoaDonChoKhachHang,
    }
}
