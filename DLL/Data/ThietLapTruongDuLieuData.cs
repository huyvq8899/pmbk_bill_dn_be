using DLL.Entity.Config;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DLL.Data
{
    public class ThietLapTruongDuLieuData
    {
        /// <summary>
        /// init data 
        /// </summary>
        /// <returns></returns>
        public List<ThietLapTruongDuLieu> InitData()
        {
            HoaDonDienTu hoaDonDienTu = new HoaDonDienTu();
            HoaDonDienTuChiTiet hoaDonDienTuChiTiet = new HoaDonDienTuChiTiet();
            const string SoNgay = "SoNgay";
            const string KhachHang = "KhachHang";
            const string HinhThucUyNhiemLapHoaDon = "HinhThucUyNhiemLapHoaDon";
            const string PhuongThucChuyenDuLieu = "PhuongThucChuyenDuLieu";
            const string ThongTinTaoCapNhat = "ThongTinTaoCapNhat";
            const string NhanVienBanHang = "NhanVienBanHang";
            const string DonViTinh = nameof(DonViTinh);
            const string HinhThucHoaDon = nameof(HinhThucHoaDon);
            const string UyNhiemLapHoaDon = nameof(UyNhiemLapHoaDon);
            const string HinhThucDieuChinh = nameof(HinhThucDieuChinh);
            const string TrangThaiThoaThuan = nameof(TrangThaiThoaThuan);
            const string ThongTinTao = nameof(ThongTinTao);
            const string ThongTinCapNhat = nameof(ThongTinCapNhat);
            const string ThongTinSaiSot = nameof(ThongTinSaiSot);

            #region data
            List<ThietLapTruongDuLieu> data = new List<ThietLapTruongDuLieu>
            {
                new ThietLapTruongDuLieu
                 {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.KyHieu),
                    TenTruong = "Ký hiệu mẫu số hóa đơn/Ký hiệu hóa đơn",
                    TenTruongHienThi = "Ký hiệu",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = SoNgay,
                    TenTruong = "Số hóa đơn/Ngày hóa đơn",
                    TenTruongHienThi = "Số/Ngày",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.None,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.MaKhachHang),
                    TenTruong = "Mã khách hàng",
                    TenTruongHienThi = "Mã khách hàng",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 140,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = KhachHang,
                    TenTruong = "Người mua hàng/Mã số thuế/Tên khách hàng",
                    TenTruongHienThi = "Khách hàng",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 450,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.DiaChi),
                    TenTruong = "Địa chỉ",
                    TenTruongHienThi = "Địa chỉ",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThai),
                    TenTruong = "Trạng thái hóa đơn/Mã tra cứu",
                    TenTruongHienThi = "Trạng thái hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 250,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiQuyTrinh),
                    TenTruong = "Trạng thái quy trình/Mã CQT cấp",
                    TenTruongHienThi = "Trạng thái quy trình",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 210,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TongTienThanhToan),
                    TenTruong = "Tổng tiền thanh toán/Loại tiền",
                    TenTruongHienThi = "Tổng tiền thanh toán",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.None,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiGuiHoaDon),
                    TenTruong = "Trạng thái gửi hóa đơn",
                    TenTruongHienThi = "Gửi hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.HoTenNguoiNhanHD),
                    TenTruong = "Tên người nhận",
                    TenTruongHienThi = "Tên người nhận",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.EmailNguoiNhanHD),
                    TenTruong = "Email người nhận",
                    TenTruongHienThi = "Email người nhận",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoDienThoaiNguoiNhanHD),
                    TenTruong = "Số điện thoại người nhận",
                    TenTruongHienThi = "Số điện thoại người nhận",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoLanChuyenDoi),
                    TenTruong = "Số lần chuyển thành hóa đơn giấy",
                    TenTruongHienThi = "Chuyển đổi",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 120,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TaiLieuDinhKem),
                    TenTruong = "Đính kèm",
                    TenTruongHienThi = "Đính kèm",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 80,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LyDoXoaBo),
                    TenTruong = "Lý do xóa bỏ",
                    TenTruongHienThi = "Lý do xóa bỏ",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(ThongTinSaiSot),
                    TenTruong = "Thông báo hóa đơn điện tử có sai sót",
                    TenTruongHienThi = "Thông báo hóa đơn điện tử có sai sót",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 380,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = NhanVienBanHang,
                    TenTruong = "NV bán hàng",
                    TenTruongHienThi = "NV bán hàng",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = HinhThucUyNhiemLapHoaDon,
                    TenTruong = "Hình thức hóa đơn/Ủy nhiệm lập hóa đơn",
                    TenTruongHienThi = "Hình thức/Ủy nhiệm lập hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = PhuongThucChuyenDuLieu,
                    TenTruong = "Phương thức chuyển dữ liệu",
                    TenTruongHienThi = "Phương thức chuyển dữ liệu",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 230,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LoaiHoaDon),
                    TenTruong = "Loại hóa đơn",
                    TenTruongHienThi = "Loại hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                //new ThietLapTruongDuLieu
                //{
                //    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                //    MaTruong = null,
                //    TenCot = TrangThaiThoaThuan,
                //    TenTruong = "Trạng thái thỏa thuận",
                //    TenTruongHienThi = "Trạng thái thỏa thuận",
                //    LoaiHoaDon = LoaiHoaDon.None,
                //    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                //    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                //    GhiChu = null,
                //    DoRong = 150,
                //    HienThi = false
                //},
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LoaiChungTu),
                    TenTruong = "Loại chứng từ",
                    TenTruongHienThi = "Loại chứng từ",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = ThongTinTaoCapNhat,
                    TenTruong = "Thông tin tạo/Thông tin cập nhật",
                    TenTruongHienThi = "Thông tin tạo/cập nhật",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung1),
                    TenTruong = "Trường thông tin bổ sung 1",
                    TenTruongHienThi = "Trường thông tin bổ sung 1",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung2),
                    TenTruong = "Trường thông tin bổ sung 2",
                    TenTruongHienThi = "Trường thông tin bổ sung 2",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung3),
                    TenTruong = "Trường thông tin bổ sung 3",
                    TenTruongHienThi = "Trường thông tin bổ sung 3",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung4),
                    TenTruong = "Trường thông tin bổ sung 4",
                    TenTruongHienThi = "Trường thông tin bổ sung 4",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung5),
                    TenTruong = "Trường thông tin bổ sung 5",
                    TenTruongHienThi = "Trường thông tin bổ sung 5",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung6),
                    TenTruong = "Trường thông tin bổ sung 6",
                    TenTruongHienThi = "Trường thông tin bổ sung 6",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung7),
                    TenTruong = "Trường thông tin bổ sung 7",
                    TenTruongHienThi = "Trường thông tin bổ sung 7",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung8),
                    TenTruong = "Trường thông tin bổ sung 8",
                    TenTruongHienThi = "Trường thông tin bổ sung 8",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung9),
                    TenTruong = "Trường thông tin bổ sung 9",
                    TenTruongHienThi = "Trường thông tin bổ sung 9",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung10),
                    TenTruong = "Trường thông tin bổ sung 10",
                    TenTruongHienThi = "Trường thông tin bổ sung 10",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                //-------------------------- thong tin bo sung ----------------------------
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung1),
                    TenTruong = "Trường thông tin bổ sung 1",
                    TenTruongHienThi = "Trường thông tin bổ sung 1",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung2),
                    TenTruong = "Trường thông tin bổ sung 2",
                    TenTruongHienThi = "Trường thông tin bổ sung 2",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung3),
                    TenTruong = "Trường thông tin bổ sung 3",
                    TenTruongHienThi = "Trường thông tin bổ sung 3",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung4),
                    TenTruong = "Trường thông tin bổ sung 4",
                    TenTruongHienThi = "Trường thông tin bổ sung 4",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung5),
                    TenTruong = "Trường thông tin bổ sung 5",
                    TenTruongHienThi = "Trường thông tin bổ sung 5",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung6),
                    TenTruong = "Trường thông tin bổ sung 6",
                    TenTruongHienThi = "Trường thông tin bổ sung 6",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung7),
                    TenTruong = "Trường thông tin bổ sung 7",
                    TenTruongHienThi = "Trường thông tin bổ sung 7",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung8),
                    TenTruong = "Trường thông tin bổ sung 8",
                    TenTruongHienThi = "Trường thông tin bổ sung 8",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung9),
                    TenTruong = "Trường thông tin bổ sung 9",
                    TenTruongHienThi = "Trường thông tin bổ sung 9",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung10),
                    TenTruong = "Trường thông tin bổ sung 10",
                    TenTruongHienThi = "Trường thông tin bổ sung 10",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                // ---------------------------- thong tin mo rong ---------------------------------
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.STT),
                    TenTruong = "STT",
                    TenTruongHienThi = "STT",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 50,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.MaHang),
                    TenTruong = "Mã hàng",
                    TenTruongHienThi = "Mã hàng",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TenHang),
                    TenTruong = "Tên hàng",
                    TenTruongHienThi = "Tên hàng hóa, dịch vụ",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TinhChat),
                    TenTruong = "Tính chất",
                    TenTruongHienThi = "Tính chất",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 140,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.MaQuyCach),
                    TenTruong = "Mã quy cách",
                    TenTruongHienThi = "Mã quy cách",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = DonViTinh,
                    TenTruong = "ĐVT",
                    TenTruongHienThi = "ĐVT",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoLuong),
                    TenTruong = "Số lượng",
                    TenTruongHienThi = "Số lượng",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.DonGiaSauThue),
                    TenTruong = "Đơn giá sau thuế",
                    TenTruongHienThi = "Đơn giá sau thuế",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.DonGia),
                    TenTruong = "Đơn giá",
                    TenTruongHienThi = "Đơn giá",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThanhTienSauThue),
                    TenTruong = "Thành tiền sau thuế",
                    TenTruongHienThi = "Thành tiền sau thuế",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThanhTien),
                    TenTruong = "Thành tiền",
                    TenTruongHienThi = "Thành tiền",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThanhTienQuyDoi),
                    TenTruong = "Thành tiền quy đổi",
                    TenTruongHienThi = "Thành tiền quy đổi",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TyLeChietKhau),
                    TenTruong = "Tỷ lệ chiết khấu",
                    TenTruongHienThi = "Tỷ lệ chiết khấu",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.PhanTram,
                    GhiChu = null,
                    DoRong = 120,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TienChietKhau),
                    TenTruong = "Tiền chiết khấu",
                    TenTruongHienThi = "Tiền chiết khấu",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TienChietKhauQuyDoi),
                    TenTruong = "Tiền chiết khấu quy đổi",
                    TenTruongHienThi = "Tiền chiết khấu quy đổi",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 140,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThueGTGT),
                    TenTruong = "% Thuế GTGT",
                    TenTruongHienThi = "% Thuế GTGT",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.PhanTram,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TienThueGTGT),
                    TenTruong = "Tiền thuế GTGT",
                    TenTruongHienThi = "Tiền thuế GTGT",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TienThueGTGTQuyDoi),
                    TenTruong = "Tiền thuế GTGT quy đổi",
                    TenTruongHienThi = "Tiền thuế GTGT quy đổi",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoLo),
                    TenTruong = "Số lô",
                    TenTruongHienThi = "Số lô",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.HanSuDung),
                    TenTruong = "Hạn sử dụng",
                    TenTruongHienThi = "Hạn sử dụng",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoKhung),
                    TenTruong = "Số khung",
                    TenTruongHienThi = "Số khung",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoMay),
                    TenTruong = "Số máy",
                    TenTruongHienThi = "Số máy",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.XuatBanPhi),
                    TenTruong = "Xuất bản phí",
                    TenTruongHienThi = "Xuất bản phí",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.GhiChu),
                    TenTruong = "Ghi chú",
                    TenTruongHienThi = "Ghi chú",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.MaNhanVien),
                    TenTruong = "Mã nhân viên",
                    TenTruongHienThi = "Mã nhân viên",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TenNhanVien),
                    TenTruong = "Tên nhân viên",
                    TenTruongHienThi = "Tên nhân viên",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet1),
                    TenTruong = "Trường mở rộng chi tiết số 1",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 1",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet2),
                    TenTruong = "Trường mở rộng chi tiết số 2",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 2",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet3),
                    TenTruong = "Trường mở rộng chi tiết số 3",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 3",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet4),
                    TenTruong = "Trường mở rộng chi tiết số 4",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 4",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet5),
                    TenTruong = "Trường mở rộng chi tiết số 5",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 5",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet6),
                    TenTruong = "Trường mở rộng chi tiết số 6",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 6",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet7),
                    TenTruong = "Trường mở rộng chi tiết số 7",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 7",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet8),
                    TenTruong = "Trường mở rộng chi tiết số 8",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 8",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet9),
                    TenTruong = "Trường mở rộng chi tiết số 9",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 9",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet10),
                    TenTruong = "Trường mở rộng chi tiết số 10",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 10",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
            };
            #endregion

            // Nhóm bảng kê
            var nhomBangKes = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomBangKe).ToList();
            for (int i = 1; i <= nhomBangKes.Count; i++)
            {
                nhomBangKes[i - 1].STT = i;
            }

            // Nhóm tab
            var nhomThongTinNguoiMuaGTGTs = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomThongTinNguoiMua).ToList();
            for (int i = 1; i <= nhomThongTinNguoiMuaGTGTs.Count; i++)
            {
                ThietLapTruongDuLieu item = nhomThongTinNguoiMuaGTGTs[i - 1];
                item.STT = i;

                var cloneBanHang = (ThietLapTruongDuLieu)item.Clone();
                cloneBanHang.ThietLapTruongDuLieuId = Guid.NewGuid().ToString();
                cloneBanHang.LoaiHoaDon = LoaiHoaDon.HoaDonBanHang;
                data.Add(cloneBanHang);
            }

            // Nhóm hàng hóa dịch vụ
            var nhomHHDVGTGTs = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomHangHoaDichVu).ToList();
            for (int i = 1; i <= nhomHHDVGTGTs.Count; i++)
            {
                ThietLapTruongDuLieu item = nhomHHDVGTGTs[i - 1];
                item.MaTruong = $"HHDV {i}";
                item.STT = i;

                var cloneBanHang = (ThietLapTruongDuLieu)item.Clone();
                cloneBanHang.ThietLapTruongDuLieuId = Guid.NewGuid().ToString();
                cloneBanHang.LoaiHoaDon = LoaiHoaDon.HoaDonBanHang;
                if (cloneBanHang.TenCot != nameof(hoaDonDienTuChiTiet.ThueGTGT) &&
                    cloneBanHang.TenCot != nameof(hoaDonDienTuChiTiet.TienThueGTGT) &&
                    cloneBanHang.TenCot != nameof(hoaDonDienTuChiTiet.TienThueGTGTQuyDoi))
                {
                    data.Add(cloneBanHang);
                }
                else
                {
                    if (cloneBanHang.TenCot == nameof(hoaDonDienTuChiTiet.ThueGTGT))
                    {
                        cloneBanHang.MaTruong = $"HHDV 37";
                        cloneBanHang.STT = 37;
                        cloneBanHang.TenCot = nameof(hoaDonDienTuChiTiet.IsMatHangDuocGiam);
                        cloneBanHang.TenTruong = "Mặt hàng giảm";
                        cloneBanHang.TenTruongHienThi = "Mặt hàng giảm";
                        cloneBanHang.DoRong = 130;
                        data.Add(cloneBanHang);

                        cloneBanHang = (ThietLapTruongDuLieu)item.Clone();
                        cloneBanHang.ThietLapTruongDuLieuId = Guid.NewGuid().ToString();
                        cloneBanHang.LoaiHoaDon = LoaiHoaDon.HoaDonBanHang;
                        cloneBanHang.MaTruong = $"HHDV 38";
                        cloneBanHang.STT = 38;
                        cloneBanHang.TenCot = nameof(hoaDonDienTuChiTiet.TyLePhanTramDoanhThu);
                        cloneBanHang.TenTruong = "Tỷ lệ % doanh thu";
                        cloneBanHang.TenTruongHienThi = "Tỷ lệ % doanh thu";
                        cloneBanHang.DoRong = 120;
                        data.Add(cloneBanHang);
                    }
                    else if (cloneBanHang.TenCot == nameof(hoaDonDienTuChiTiet.TienThueGTGT))
                    {
                        cloneBanHang.MaTruong = $"HHDV 39";
                        cloneBanHang.STT = 39;
                        cloneBanHang.TenCot = nameof(hoaDonDienTuChiTiet.TienGiam);
                        cloneBanHang.TenTruong = "Tiền giảm 20% mức tỷ lệ";
                        cloneBanHang.TenTruongHienThi = "Tiền giảm 20% mức tỷ lệ";
                        cloneBanHang.DoRong = 180;
                        data.Add(cloneBanHang);
                    }
                    else if (cloneBanHang.TenCot == nameof(hoaDonDienTuChiTiet.TienThueGTGTQuyDoi))
                    {
                        cloneBanHang.MaTruong = $"HHDV 40";
                        cloneBanHang.STT = 40;
                        cloneBanHang.TenCot = nameof(hoaDonDienTuChiTiet.TienGiamQuyDoi);
                        cloneBanHang.TenTruong = "Tiền giảm 20% mức tỷ lệ quy đổi";
                        cloneBanHang.TenTruongHienThi = "Tiền giảm 20% mức tỷ lệ quy đổi";
                        cloneBanHang.DoRong = 200;
                        data.Add(cloneBanHang);
                    }
                }
            }

            return data;
        }

        public List<ThietLapTruongDuLieu> InitDataPXK()
        {
            HoaDonDienTu hoaDonDienTu = new HoaDonDienTu();
            HoaDonDienTuChiTiet hoaDonDienTuChiTiet = new HoaDonDienTuChiTiet();
            const string SoNgay = "SoNgay";
            const string KhachHang = "KhachHang";
            const string HinhThucUyNhiemLapHoaDon = "HinhThucUyNhiemLapHoaDon";
            const string PhuongThucChuyenDuLieu = "PhuongThucChuyenDuLieu";
            const string ThongTinTaoCapNhat = "ThongTinTaoCapNhat";
            const string NhanVienBanHang = "NhanVienBanHang";
            const string DonViTinh = nameof(DonViTinh);
            const string HinhThucHoaDon = nameof(HinhThucHoaDon);
            const string UyNhiemLapHoaDon = nameof(UyNhiemLapHoaDon);
            const string HinhThucDieuChinh = nameof(HinhThucDieuChinh);
            const string TrangThaiThoaThuan = nameof(TrangThaiThoaThuan);
            const string ThongTinTao = nameof(ThongTinTao);
            const string ThongTinCapNhat = nameof(ThongTinCapNhat);
            const string ThongTinSaiSot = nameof(ThongTinSaiSot);
            const string LenhDieuDongHopDongKinhTe = nameof(LenhDieuDongHopDongKinhTe);
            const string VeViecDienGiai = nameof(VeViecDienGiai);
            const string ThongTinXuatHang = nameof(ThongTinXuatHang);
            const string ThongTinVanChuyen = nameof(ThongTinVanChuyen);

            #region data
            List<ThietLapTruongDuLieu> data = new List<ThietLapTruongDuLieu>
            {
                new ThietLapTruongDuLieu
                 {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.KyHieu),
                    TenTruong = "Ký hiệu mẫu số hóa đơn/Ký hiệu hóa đơn",
                    TenTruongHienThi = "Ký hiệu",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = SoNgay,
                    TenTruong = "Số hóa đơn/Ngày hóa đơn",
                    TenTruongHienThi = "Số/Ngày",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.None,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.MaKhachHang),
                    TenTruong = "Mã khách hàng",
                    TenTruongHienThi = "Mã đơn vị nhận hàng/Mã đại lý",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 140,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = KhachHang,
                    TenTruong = "Người mua hàng/Mã số thuế/Tên khách hàng",
                    TenTruongHienThi = "Đơn vị nhận hàng/Đại lý",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 450,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.DiaChi),
                    TenTruong = "Địa chỉ",
                    TenTruongHienThi = "Địa chỉ kho nhận hàng",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThai),
                    TenTruong = "Trạng thái hóa đơn/Mã tra cứu",
                    TenTruongHienThi = "Trạng thái PXK",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 250,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiQuyTrinh),
                    TenTruong = "Trạng thái quy trình/Mã CQT cấp",
                    TenTruongHienThi = "Trạng thái quy trình",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 210,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TongTienThanhToan),
                    TenTruong = "Tổng tiền thanh toán/Loại tiền",
                    TenTruongHienThi = "Tổng tiền thanh toán",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.None,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiGuiHoaDon),
                    TenTruong = "Trạng thái gửi hóa đơn",
                    TenTruongHienThi = "Gửi PXK",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.HoTenNguoiNhanHD),
                    TenTruong = "Tên người nhận",
                    TenTruongHienThi = "Tên người nhận",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.EmailNguoiNhanHD),
                    TenTruong = "Email người nhận",
                    TenTruongHienThi = "Email người nhận",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoDienThoaiNguoiNhanHD),
                    TenTruong = "Số điện thoại người nhận",
                    TenTruongHienThi = "Số điện thoại người nhận",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoLanChuyenDoi),
                    TenTruong = "Số lần chuyển thành hóa đơn giấy",
                    TenTruongHienThi = "Chuyển đổi",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 120,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TaiLieuDinhKem),
                    TenTruong = "Đính kèm",
                    TenTruongHienThi = "Đính kèm",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 80,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LyDoXoaBo),
                    TenTruong = "Lý do xóa bỏ",
                    TenTruongHienThi = "Lý do xóa bỏ",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(ThongTinSaiSot),
                    TenTruong = "Thông báo hóa đơn điện tử có sai sót",
                    TenTruongHienThi = "Thông báo PXK điện tử có sai sót",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 380,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = NhanVienBanHang,
                    TenTruong = "NV bán hàng",
                    TenTruongHienThi = "NV bán hàng",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = HinhThucUyNhiemLapHoaDon,
                    TenTruong = "Hình thức hóa đơn/Ủy nhiệm lập hóa đơn",
                    TenTruongHienThi = "Hình thức/Ủy nhiệm lập PXK",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = PhuongThucChuyenDuLieu,
                    TenTruong = "Phương thức chuyển dữ liệu",
                    TenTruongHienThi = "Phương thức chuyển dữ liệu",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 230,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LoaiHoaDon),
                    TenTruong = "Loại hóa đơn",
                    TenTruongHienThi = "Loại PXK",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = LenhDieuDongHopDongKinhTe,
                    TenTruong = "Lệnh điều động/Hợp đồng kinh tế",
                    TenTruongHienThi = "Lệnh điều động/Hợp đồng kinh tế",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = VeViecDienGiai,
                    TenTruong = "Về việc/Diễn giải",
                    TenTruongHienThi = "Về việc/Diễn giải",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = ThongTinXuatHang,
                    TenTruong = "Thông tin xuất hàng",
                    TenTruongHienThi = "Thông tin xuất hàng",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = ThongTinVanChuyen,
                    TenTruong = "Thông tin vận chuyển",
                    TenTruongHienThi = "Thông tin vận chuyển",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 200,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.LoaiChungTu),
                    TenTruong = "Loại chứng từ",
                    TenTruongHienThi = "Loại chứng từ",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = ThongTinTaoCapNhat,
                    TenTruong = "Thông tin tạo/Thông tin cập nhật",
                    TenTruongHienThi = "Thông tin tạo/cập nhật",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung1),
                    TenTruong = "Trường thông tin bổ sung 1",
                    TenTruongHienThi = "Trường thông tin bổ sung 1",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung2),
                    TenTruong = "Trường thông tin bổ sung 2",
                    TenTruongHienThi = "Trường thông tin bổ sung 2",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung3),
                    TenTruong = "Trường thông tin bổ sung 3",
                    TenTruongHienThi = "Trường thông tin bổ sung 3",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung4),
                    TenTruong = "Trường thông tin bổ sung 4",
                    TenTruongHienThi = "Trường thông tin bổ sung 4",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung5),
                    TenTruong = "Trường thông tin bổ sung 5",
                    TenTruongHienThi = "Trường thông tin bổ sung 5",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung6),
                    TenTruong = "Trường thông tin bổ sung 6",
                    TenTruongHienThi = "Trường thông tin bổ sung 6",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung7),
                    TenTruong = "Trường thông tin bổ sung 7",
                    TenTruongHienThi = "Trường thông tin bổ sung 7",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung8),
                    TenTruong = "Trường thông tin bổ sung 8",
                    TenTruongHienThi = "Trường thông tin bổ sung 8",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung9),
                    TenTruong = "Trường thông tin bổ sung 9",
                    TenTruongHienThi = "Trường thông tin bổ sung 9",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung10),
                    TenTruong = "Trường thông tin bổ sung 10",
                    TenTruongHienThi = "Trường thông tin bổ sung 10",
                    LoaiHoaDon = LoaiHoaDon.PhieuXuatKho,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = false
                },
                //-------------------------- thong tin bo sung ----------------------------
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung1),
                    TenTruong = "Trường thông tin bổ sung 1",
                    TenTruongHienThi = "Trường thông tin bổ sung 1",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung2),
                    TenTruong = "Trường thông tin bổ sung 2",
                    TenTruongHienThi = "Trường thông tin bổ sung 2",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung3),
                    TenTruong = "Trường thông tin bổ sung 3",
                    TenTruongHienThi = "Trường thông tin bổ sung 3",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung4),
                    TenTruong = "Trường thông tin bổ sung 4",
                    TenTruongHienThi = "Trường thông tin bổ sung 4",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung5),
                    TenTruong = "Trường thông tin bổ sung 5",
                    TenTruongHienThi = "Trường thông tin bổ sung 5",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung6),
                    TenTruong = "Trường thông tin bổ sung 6",
                    TenTruongHienThi = "Trường thông tin bổ sung 6",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung7),
                    TenTruong = "Trường thông tin bổ sung 7",
                    TenTruongHienThi = "Trường thông tin bổ sung 7",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung8),
                    TenTruong = "Trường thông tin bổ sung 8",
                    TenTruongHienThi = "Trường thông tin bổ sung 8",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung9),
                    TenTruong = "Trường thông tin bổ sung 9",
                    TenTruongHienThi = "Trường thông tin bổ sung 9",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TruongThongTinBoSung10),
                    TenTruong = "Trường thông tin bổ sung 10",
                    TenTruongHienThi = "Trường thông tin bổ sung 10",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomThongTinNguoiMua,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = null,
                    HienThi = true
                },
                // ---------------------------- thong tin mo rong ---------------------------------
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.STT),
                    TenTruong = "STT",
                    TenTruongHienThi = "STT",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 50,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.MaHang),
                    TenTruong = "Mã hàng",
                    TenTruongHienThi = "Mã hàng",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TenHang),
                    TenTruong = "Tên hàng",
                    TenTruongHienThi = "Tên hàng",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TinhChat),
                    TenTruong = "Tính chất",
                    TenTruongHienThi = "Tính chất",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 140,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.MaQuyCach),
                    TenTruong = "Mã quy cách",
                    TenTruongHienThi = "Mã quy cách",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = DonViTinh,
                    TenTruong = "ĐVT",
                    TenTruongHienThi = "ĐVT",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 100,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoLuong),
                    TenTruong = "Số lượng thực xuất",
                    TenTruongHienThi = "Số lượng thực xuất",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoLuongNhap),
                    TenTruong = "Số lượng thực nhập",
                    TenTruongHienThi = "Số lượng thực nhập",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.DonGia),
                    TenTruong = "Đơn giá",
                    TenTruongHienThi = "Đơn giá",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThanhTien),
                    TenTruong = "Thành tiền",
                    TenTruongHienThi = "Thành tiền",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.ThanhTienQuyDoi),
                    TenTruong = "Thành tiền quy đổi",
                    TenTruongHienThi = "Thành tiền quy đổi",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoLo),
                    TenTruong = "Số lô",
                    TenTruongHienThi = "Số lô",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.HanSuDung),
                    TenTruong = "Hạn sử dụng",
                    TenTruongHienThi = "Hạn sử dụng",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoKhung),
                    TenTruong = "Số khung",
                    TenTruongHienThi = "Số khung",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.SoMay),
                    TenTruong = "Số máy",
                    TenTruongHienThi = "Số máy",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.XuatBanPhi),
                    TenTruong = "Xuất bản phí",
                    TenTruongHienThi = "Xuất bản phí",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.GhiChu),
                    TenTruong = "Ghi chú",
                    TenTruongHienThi = "Ghi chú",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet1),
                    TenTruong = "Trường mở rộng chi tiết số 1",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 1",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet2),
                    TenTruong = "Trường mở rộng chi tiết số 2",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 2",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet3),
                    TenTruong = "Trường mở rộng chi tiết số 3",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 3",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet4),
                    TenTruong = "Trường mở rộng chi tiết số 4",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 4",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet5),
                    TenTruong = "Trường mở rộng chi tiết số 5",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 5",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet6),
                    TenTruong = "Trường mở rộng chi tiết số 6",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 6",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet7),
                    TenTruong = "Trường mở rộng chi tiết số 7",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 7",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet8),
                    TenTruong = "Trường mở rộng chi tiết số 8",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 8",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet9),
                    TenTruong = "Trường mở rộng chi tiết số 9",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 9",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    TenCot = nameof(hoaDonDienTuChiTiet.TruongMoRongChiTiet10),
                    TenTruong = "Trường mở rộng chi tiết số 10",
                    TenTruongHienThi = "Trường mở rộng chi tiết số 10",
                    LoaiHoaDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = false
                },
            };
            #endregion

            // Nhóm bảng kê
            var nhomBangKes = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomBangKe).ToList();
            for (int i = 1; i <= nhomBangKes.Count; i++)
            {
                nhomBangKes[i - 1].STT = i;
            }

            // Nhóm tab
            var nhomThongTinNguoiMuaGTGTs = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomThongTinNguoiMua).ToList();
            for (int i = 1; i <= nhomThongTinNguoiMuaGTGTs.Count; i++)
            {
                ThietLapTruongDuLieu item = nhomThongTinNguoiMuaGTGTs[i - 1];
                item.STT = i;

                var cloneGuiBanDaiLy = (ThietLapTruongDuLieu)item.Clone();
                cloneGuiBanDaiLy.ThietLapTruongDuLieuId = Guid.NewGuid().ToString();
                cloneGuiBanDaiLy.LoaiHoaDon = LoaiHoaDon.PXKHangGuiBanDaiLy;
                data.Add(cloneGuiBanDaiLy);
            }

            // Nhóm hàng hóa dịch vụ
            var nhomHHDVGTGTs = data.Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomHangHoaDichVu && x.TenCot != nameof(hoaDonDienTuChiTiet.SoLuongNhap)).ToList();
            for (int i = 1; i <= nhomHHDVGTGTs.Count; i++)
            {
                ThietLapTruongDuLieu item = nhomHHDVGTGTs[i - 1];
                item.MaTruong = $"HHDV {i}";
                item.STT = i;

                var cloneGuiBanDaiLy = (ThietLapTruongDuLieu)item.Clone();
                cloneGuiBanDaiLy.ThietLapTruongDuLieuId = Guid.NewGuid().ToString();
                cloneGuiBanDaiLy.LoaiHoaDon = LoaiHoaDon.PXKHangGuiBanDaiLy;

                if (cloneGuiBanDaiLy.TenCot == nameof(hoaDonDienTuChiTiet.SoLuong))
                {
                    cloneGuiBanDaiLy.TenCot = "Số lượng";
                    cloneGuiBanDaiLy.TenTruongHienThi = "Số lượng";
                    cloneGuiBanDaiLy.DoRong = 100;
                }

                data.Add(cloneGuiBanDaiLy);
            }

            return data;
        }

        public string QueryInsertIsMatHangDuocGiam()
        {
            var hoaDonDienTuChiTiet = new HoaDonDienTuChiTiet();
            const string IsMatHangDuocGiam = nameof(hoaDonDienTuChiTiet.IsMatHangDuocGiam);
            const string TyLePhanTramDoanhThu = nameof(hoaDonDienTuChiTiet.TyLePhanTramDoanhThu);
            const string TienGiam = nameof(hoaDonDienTuChiTiet.TienGiam);
            const string TienGiamQuyDoi = nameof(hoaDonDienTuChiTiet.TienGiamQuyDoi);

            var tienGiams = InitData()
                .Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomHangHoaDichVu && (x.TenCot == IsMatHangDuocGiam || x.TenCot == TyLePhanTramDoanhThu || x.TenCot == TienGiam || x.TenCot == TienGiamQuyDoi))
                .ToList();

            var result = $"DELETE FROM ThietLapTruongDuLieus WHERE TenCot = '{TyLePhanTramDoanhThu}' OR TenCot = '{TienGiam}' OR TenCot = '{TienGiamQuyDoi}'";
            result += Query(tienGiams);
            return result;
        }

        public string QueryUpdateNhomBangKe()
        {
            string result = "DELETE FROM ThietLapTruongDuLieus WHERE LoaiTruongDuLieu = 0;";
            var list = InitData().Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomBangKe).ToList();
            result += Query(list);
            return result;
        }

        public string QueryInsertData()
        {
            var list = InitData();
            var result = Query(list);
            return result;
        }

        public string QueryInsertDataPXK()
        {
            var list = InitDataPXK();
            var result = Query(list);
            return result;
        }

        public string QueryUpdateTruongDuLieuTheoYeuCauCuaSep()
        {
            var list = InitData().Where(x => x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomBangKe).ToList();
            var result = Query(list);
            result += "UPDATE ThietLapTruongDuLieus SET DoRong = 140 WHERE TenCot = 'TinhChat' AND LoaiTruongDuLieu = 2";
            return result;
        }

        public string QueryInsertTienGiam()
        {
            var hddtct = new HoaDonDienTuChiTiet();

            var list = InitData().Where(x => x.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomHangHoaDichVu).ToList();
            var listTienGiam = new List<ThietLapTruongDuLieu>();
            var listOtherAfter = new List<ThietLapTruongDuLieu>();
            var indexTienGiam = list.FindIndex(x => x.TenCot == nameof(hddtct.TienGiam));
            for (int i = indexTienGiam; i < list.Count; i++)
            {
                var item = list[i];

                if (item.TenCot == nameof(hddtct.TienGiam) || item.TenCot == nameof(hddtct.TienGiamQuyDoi))
                {
                    listTienGiam.Add(item);
                }
                else
                {
                    listOtherAfter.Add(item);
                }
            }

            var result = Query(listTienGiam);
            foreach (var item in listOtherAfter)
            {
                result += $"UPDATE ThietLapTruongDuLieus SET MaTruong = '{item.MaTruong}', STT = {item.STT} WHERE TenCot = '{item.TenCot}';";
            }

            return result;
        }

        public string QueryInsertTyLePhanTramDoanhThu()
        {
            var hddtct = new HoaDonDienTuChiTiet();

            var list = InitData().Where(x => x.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && x.LoaiTruongDuLieu == LoaiTruongDuLieu.NhomHangHoaDichVu).ToList();
            var listTyLePhanTramDoanhThu = new List<ThietLapTruongDuLieu>();
            var listOtherAfter = new List<ThietLapTruongDuLieu>();
            var indexTyLePhanTram = list.FindIndex(x => x.TenCot == nameof(hddtct.TyLePhanTramDoanhThu));
            for (int i = indexTyLePhanTram; i < list.Count; i++)
            {
                var item = list[i];

                if (item.TenCot == nameof(hddtct.TyLePhanTramDoanhThu))
                {
                    listTyLePhanTramDoanhThu.Add(item);
                }
                else
                {
                    listOtherAfter.Add(item);
                }
            }

            var result = Query(listTyLePhanTramDoanhThu);
            foreach (var item in listOtherAfter)
            {
                result += $"UPDATE ThietLapTruongDuLieus SET MaTruong = '{item.MaTruong}', STT = {item.STT} WHERE TenCot = '{item.TenCot}';";
            }

            return result;
        }

        /// <summary>
        /// Query insert multi item from list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string Query(List<ThietLapTruongDuLieu> list)
        {
            var result = @"INSERT INTO [ThietLapTruongDuLieus] ([ThietLapTruongDuLieuId], 
                                                                [DoRong], 
                                                                [GhiChu], 
                                                                [HienThi], 
                                                                [KieuDuLieu], 
                                                                [LoaiHoaDon], 
                                                                [LoaiTruongDuLieu], 
                                                                [MaTruong], 
                                                                [STT], 
                                                                [TenCot], 
                                                                [TenTruong], 
                                                                [TenTruongHienThi]) VALUES ";

            var length = list.Count;
            for (int i = 0; i < length; i++)
            {
                var item = list[i];
                result += $@"('{item.ThietLapTruongDuLieuId}',";
                result += item.DoRong.HasValue ? $"{item.DoRong}," : "NULL,";
                result += $@"N'{item.GhiChu}', 
                               {(item.HienThi == true ? 1 : 0)}, 
                               {(int)item.KieuDuLieu}, 
                               {(int)item.LoaiHoaDon}, 
                               {(int)item.LoaiTruongDuLieu}, 
                               '{item.MaTruong}', 
                               {item.STT}, 
                               '{item.TenCot}', 
                               N'{item.TenTruong}', 
                               N'{item.TenTruongHienThi}')";

                if (i == length - 1)
                {
                    result += ";";
                }
                else
                {
                    result += ",";
                }
            }
            return result;
        }
    }
}
