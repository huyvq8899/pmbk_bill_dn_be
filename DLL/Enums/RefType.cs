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
        #endregion
    }
}
