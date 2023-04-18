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
        [Description("Chuyển thành hóa đơn thay thế")]
        ChuyenThanhHoaDonThayThe,
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
        GuiBienBanHuyHoaDon,
        [Description("Phát hành hóa đơn đồng loạt")]
        PhatHanhHoaDonDongLoat,
        [Description("Cập nhật số lớn nhất đã sinh đến hiện tại")]
        CapNhatSoLonNhatDaSinh,
        [Description("Gửi hóa đơn đồng loạt")]
        GuiHoaDonDongLoat,
        [Description("Gửi hóa đơn nháp đồng loạt")]
        GuiHoaDonNhapDongLoat,
        [Description("Gửi phiếu xuất kho cho khách hàng")]
        GuiPXKChoKhachHang,
        [Description("Gửi biên bản điều chỉnh phiếu xuất kho")]
        GuiBienBanDieuChinhPXK,
        [Description("Gửi thông báo phiếu xuất kho có thông tin sai sót không phải lập lại phiếu xuất kho")]
        GuiThongBaoPXKCoThongTinSaiSot,
        [Description("Gửi thông báo xóa bỏ phiếu xuất kho")]
        GuiThongBaoXoaBoPXK,
        [Description("Gửi biên bản hủy phiếu xuất kho")]
        GuiBienBanHuyPXK,
        [Description("Thêm từ BKPOS Bách Khoa")]
        ThemBoiBKPOS,
        [Description("Xác nhận hóa đơn đã gửi cho khách hàng")]
        XacNhanHoaDonDaGuiChoKhachHang,
        [Description("Gửi CQT đồng loạt")]
        GuiCqtDongLoat,
        [Description("Gửi CQT")]
        GuiCQT,
        [Description("Sửa từ BKPOS")]
        SuaBoiBKPOS,
    }
}
