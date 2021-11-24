using System.ComponentModel;

namespace Services.Enums
{
    public enum LoaiHanhDong
    {
        [Description("Đăng nhập")]
        DangNhap,
        [Description("Đăng xuất")]
        DangXuat,
        [Description("Thêm")]
        Them,
        [Description("Sửa")]
        Sua,
        [Description("Xóa")]
        Xoa,
        [Description("Nhập khẩu")]
        NhapKhau,
        [Description("Xuất khẩu")]
        XuatKhau,
        [Description("In")]
        In,
        [Description("Ký điện tử")]
        KyDienTu,
        [Description("Phát hành hóa đơn thành công")]
        PhatHanhHoaDonThanhCong,
        [Description("Phát hành hóa đơn thất bại")]
        PhatHanhHoaDonThatBai,
        [Description("Chuyển thành hóa đơn giấy")]
        ChuyenThanhHoaDonGiay,
        [Description("Gửi HĐ cho KH")]
        GuiHoaDonChoKhachHang,
        [Description("Xóa bỏ hóa đơn")]
        XoaBoHoaDon,
        [Description("Phân quyền")]
        PhanQuyen,
        [Description("Ký tờ khai")]
        KyToKhai,
        [Description("Ký tờ khai bị lỗi")]
        KyToKhaiLoi,
        [Description("Ký và gửi thông báo tới CQT")]
        KyGuiThongBaoToiCQT,
    }
}
