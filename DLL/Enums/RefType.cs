using System.ComponentModel;

namespace DLL.Enums
{
    /// <summary>
    /// ------------------------ Note ------------------------------
    /// Muốn thêm RefType thì để RefType mới thêm ở sau cùng, để không
    /// ảnh hưởng đến các RefType đã lưu trước đấy.
    /// </summary>
    public enum RefType
    {
        #region Danh mục
        [Description("Khách hàng")]
        KhachHang,
        [Description("Nhân viên")]
        NhanVien,
        [Description("Hàng hóa, dịch vụ")]
        HangHoaDichVu,
        [Description("Đơn vị tính")]
        DonViTinh,
        [Description("Loại tiền")]
        LoaiTien,
        [Description("Hình thức thanh toán")]
        HinhThucThanhToan,
        [Description("Hồ sơ hóa đơn điện tử")]
        HoSoHoaDonDienTu,
        [Description("Mẫu hóa đơn")]
        MauHoaDon,
        [Description("Quyết định áp dụng hóa đơn")]
        QuyetDinhApDungHoaDon,
        [Description("Thông báo phát hành hóa đơn")]
        ThongBaoPhatHanhHoaDon,
        [Description("Thông báo kết quả hủy hóa đơn")]
        ThongBaoKetQuaHuyHoaDon,
        [Description("Thông báo điều chỉnh thông tin hóa đơn")]
        ThongBaoDieuChinhThongTinHoaDon,
        #endregion

        #region Hóa đơn
        [Description("Hóa đơn điện tử")]
        HoaDonDienTu,
        [Description("Hóa đơn xóa bỏ")]
        HoaDonXoaBo,
        [Description("Hóa đơn thay thế")]
        HoaDonThayThe,
        [Description("Hóa đơn điều chỉnh")]
        HoaDonDieuChinh,
        [Description("Biên bản điều chỉnh hóa đơn")]
        BienBanDieuChinh,
        [Description("Biên bản hủy hóa đơn")]
        BienBanXoaBo,
        #endregion

        [Description("Hóa đơn điện tử khởi tạo từ máy tính tiền")]
        HoaDonDienTuMTT,

        #region Phiếu xuất kho
        [Description("Phiếu xuất kho điện tử")]
        PXKDienTu,
        [Description("Phiếu xuất kho xóa bỏ")]
        PXKXoaBo,
        [Description("Phiếu xuất kho thay thế")]
        PXKThayThe,
        [Description("Phiếu xuất kho điều chỉnh")]
        PXKDieuChinh,
        [Description("Biên bản điều chỉnh phiếu xuất kho")]
        BienBanDieuChinhPXK,
        [Description("Biên bản hủy phiếu xuất kho")]
        BienBanXoaBoPXK,
        #endregion

        #region Hệ thống
        [Description("Quản lý người dùng")]
        NguoiDung,
        [Description("Bảo mật")]
        DangNhap,
        [Description("Bảo mật")]
        DangXuat,
        [Description("Nhập dữ liệu từ Excel")]
        NhapKhauTuExcel,
        [Description("Phân quyền chức năng")]
        PhanQuyenChucNang,
        #endregion

        [Description("Tờ khai đăng ký thay đổi thông tin sử dụng hóa đơn")]
        ToKhaiDangKyThayDoiThongTinSuDungHoaDon,
        [Description("Thông báo hóa đơn sai sót")]
        ThongBaoHoaDonSaiSot,
        [Description("Thông điệp chung")]
        ThongDiepChung,
        [Description("Bảng tổng hợp dữ liệu")]
        BangTongHopDuLieu,
        [Description("Thông điệp gửi nhận CQT")]
        ThongDiepGuiNhanCQT,

        [Description("Tùy chọn")]
        TuyChon,
        [Description("Thiết lập email gửi hóa đơn")]
        ThietLapEmailGuiHoaDon,
        [Description("Email gửi hóa đơn")]
        EmailGuiHoaDon,
        [Description("Email gửi phiếu xuất kho")]
        EmailGuiPXK,
        [Description("Thông tin hóa đơn")]
        ThongTinHoaDon
    }
}
