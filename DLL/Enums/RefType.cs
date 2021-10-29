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

        #region Quy định kỹ thuật
        [Description("Thông tin chung")]
        ThongDiepChung,
        [Description("Thông điệp tờ khai")]
        ThongDiepToKhai,
        [Description("Bảng tổng hợp dữ liệu")]
        BangTongHopDuLieu,
        #endregion

        [Description("Thông điệp gửi nhận CQT")]
        ThongDiepGuiNhanCQT
    }
}
