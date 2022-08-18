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
        [Description("Phát hành hóa đơn")]
        PhatHanhHoaDonThanhCong,
        [Description("Phát hành hóa đơn không thành công")]
        PhatHanhHoaDonThatBai,
        [Description("Chuyển thành hóa đơn giấy")]
        ChuyenThanhHoaDonGiay,
        [Description("Gửi hóa đơn cho khách hàng")]
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
        [Description("Nhân bản")]
        NhanBan,
        [Description("Cập nhật ngày hóa đơn")]
        CapNhatNgayHoaDon,
        [Description("Gửi biên bản điều chỉnh hóa đơn")]
        GuiBienBanDieuChinhHoaDon,
        [Description("Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn")]
        GuiThongBaoHoaDonCoThongTinSaiSot,
        [Description("Gửi thông báo xóa bỏ hóa đơn")]
        GuiThongBaoXoaBoHoaDon,
        [Description("Gửi biên bản hủy hóa đơn")]
        GuiBienBanHuyHoaDon
    }
}
