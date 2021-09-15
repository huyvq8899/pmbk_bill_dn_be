using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DLL.Entity.Config
{
    public class ThietLapTruongDuLieu : ICloneable
    {
        public string ThietLapTruongDuLieuId { get; set; }
        public string MaTruong { get; set; }
        public string TenCot { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public LoaiTruongDuLieu LoaiTruongDuLieu { get; set; }
        public KieuDuLieuThietLapTuyChinh KieuDuLieu { get; set; }
        public string GhiChu { get; set; }
        public int? DoRong { get; set; }
        public int STT { get; set; }
        public bool HienThi { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public List<ThietLapTruongDuLieu> InitData()
        {
            HoaDonDienTu hoaDonDienTu = new HoaDonDienTu();
            HoaDonDienTuChiTiet hoaDonDienTuChiTiet = new HoaDonDienTuChiTiet();

            #region data
            List<ThietLapTruongDuLieu> data = new List<ThietLapTruongDuLieu>
            {
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.NgayHoaDon),
                    TenTruong = "Ngày hóa đơn",
                    TenTruongHienThi = "Ngày hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 120,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoHoaDon),
                    TenTruong = "Số hóa đơn",
                    TenTruongHienThi = "Số hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.MauSo),
                    TenTruong = "Ký hiệu mẫu số hóa đơn",
                    TenTruongHienThi = "Ký hiệu mẫu số hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 190,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.KyHieu),
                    TenTruong = "Ký hiệu hóa đơn",
                    TenTruongHienThi = "Ký hiệu hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
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
                    TenCot = nameof(hoaDonDienTu.TenKhachHang),
                    TenTruong = "Tên khách hàng",
                    TenTruongHienThi = "Tên khách hàng",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 180,
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
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.MaSoThue),
                    TenTruong = "Mã số thuế",
                    TenTruongHienThi = "Mã số thuế",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.HoTenNguoiMuaHang),
                    TenTruong = "Người mua hàng",
                    TenTruongHienThi = "Người mua hàng",
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
                    TenCot = nameof(hoaDonDienTu.TenNhanVienBanHang),
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
                    TenCot = nameof(hoaDonDienTu.TongTienThanhToan),
                    TenTruong = "Tổng tiền thanh toán",
                    TenTruongHienThi = "Tổng tiền thanh toán",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.TienTe,
                    GhiChu = null,
                    DoRong = 170,
                    HienThi = true
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
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThai),
                    TenTruong = "Trạng thái hóa đơn",
                    TenTruongHienThi = "Trạng thái hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiPhatHanh),
                    TenTruong = "Trạng thái phát hành",
                    TenTruongHienThi = "Trạng thái phát hành",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.MaTraCuu),
                    TenTruong = "Mã tra cứu",
                    TenTruongHienThi = "Mã tra cứu",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 130,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TrangThaiGuiHoaDon),
                    TenTruong = "Trạng thái gửi hóa đơn",
                    TenTruongHienThi = "Trạng thái gửi hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 170,
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
                    TenCot = nameof(hoaDonDienTu.KhachHangDaNhan),
                    TenTruong = "Khách hàng đã nhận hóa đơn",
                    TenTruongHienThi = "Khách hàng đã nhận hóa đơn",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Bool,
                    GhiChu = null,
                    DoRong = 180,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.SoLanChuyenDoi),
                    TenTruong = "Số lần chuyển thành hóa đơn giấy",
                    TenTruongHienThi = "Số lần chuyển thành hóa đơn giấy",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.SoLuong,
                    GhiChu = null,
                    DoRong = 220,
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
                    HienThi = true
                },
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
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.NgayLap),
                    TenTruong = "Ngày lập",
                    TenTruongHienThi = "Ngày lập",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Ngay,
                    GhiChu = null,
                    DoRong = 120,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.NguoiLap),
                    TenTruong = "Người lập",
                    TenTruongHienThi = "Người lập",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
                    HienThi = true
                },
                new ThietLapTruongDuLieu
                {
                    ThietLapTruongDuLieuId = Guid.NewGuid().ToString(),
                    MaTruong = null,
                    TenCot = nameof(hoaDonDienTu.TaiLieuDinhKem),
                    TenTruong = "Tài liệu đính kèm",
                    TenTruongHienThi = "Tài liệu đính kèm",
                    LoaiHoaDon = LoaiHoaDon.None,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomBangKe,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Chu,
                    GhiChu = null,
                    DoRong = 150,
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
                    TenCot = nameof(hoaDonDienTuChiTiet.HangKhuyenMai),
                    TenTruong = "Hàng khuyến mại",
                    TenTruongHienThi = "Hàng khuyến mại",
                    LoaiHoaDon = LoaiHoaDon.HoaDonGTGT,
                    LoaiTruongDuLieu = LoaiTruongDuLieu.NhomHangHoaDichVu,
                    KieuDuLieu = KieuDuLieuThietLapTuyChinh.Bool,
                    GhiChu = null,
                    DoRong = 120,
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
                    TenCot = nameof(hoaDonDienTuChiTiet.DonViTinh),
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
            }

            return data;
        }
    }
}
