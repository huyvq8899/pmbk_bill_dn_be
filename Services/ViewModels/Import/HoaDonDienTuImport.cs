using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.Import
{
    public class HoaDonDienTuImport : ICloneable
    {
        public HoaDonDienTuImport()
        {
            HoaDonChiTiet = new HoaDonDienTuChiTietViewModel();
        }

        public int STT { get; set; }

        public string StrNgayHoaDon { get; set; }

        public DateTime? NgayHoaDon { get; set; }

        public string SoHoaDon { get; set; }

        public string MauSo { get; set; }

        public string KyHieu { get; set; }

        public string BoKyHieuHoaDonId { get; set; }

        public string KhachHangId { get; set; }

        public string MaKhachHang { get; set; }

        public string TenKhachHang { get; set; }

        public string DiaChi { get; set; }

        public string MaSoThue { get; set; }

        public string HoTenNguoiMuaHang { get; set; }

        public string SoDienThoaiNguoiMuaHang { get; set; }

        public string EmailNguoiMuaHang { get; set; }

        public string TenNganHang { get; set; }

        public string HoTenNguoiNhanHD { get; set; }

        public string EmailNguoiNhanHD { get; set; }

        public string SoDienThoaiNguoiNhanHD { get; set; }

        public string SoTaiKhoanNganHang { get; set; }

        public string HinhThucThanhToanId { get; set; }

        public string NhanVienBanHangId { get; set; }

        public string MaNhanVienBanHang { get; set; }

        public string TenNhanVienBanHang { get; set; }

        public string LoaiTienId { get; set; }

        public string MaLoaiTien { get; set; }

        public decimal TyGia { get; set; }

        public int? TrangThai { get; set; } // DLL.Enums.TrangThaiHoaDon

        public int? TrangThaiQuyTrinh { get; set; } // DLL.Enums.TrangThaiQuyTrinh

        public string MaTraCuu { get; set; }

        public int? TrangThaiGuiHoaDon { get; set; } // DLL.Enums.TrangThaiGuiHoaDon

        public int? LoaiHoaDon { get; set; } // DLL.Enums.LoaiHoaDon

        public List<HoaDonDienTuChiTietViewModel> HoaDonChiTiets { get; set; }

        public HoaDonDienTuChiTietViewModel HoaDonChiTiet { get; set; }

        public string FileChuaKy { get; set; }

        public string FileDaKy { get; set; }

        public string XMLChuaKy { get; set; }

        public string XMLDaKy { get; set; }

        public string MaCuaCQT { get; set; }

        public DateTime? NgayKy { get; set; }

        ////////////////////////////////////////////////
        public decimal? TongTienHang { get; set; }

        public decimal? TongTienChietKhau { get; set; }

        public decimal? TongTienThueGTGT { get; set; }

        public decimal? TongTienThanhToan { get; set; }

        public decimal? TongTienHangQuyDoi { get; set; }

        public decimal? TongTienChietKhauQuyDoi { get; set; }

        public decimal? TongTienThueGTGTQuyDoi { get; set; }

        public decimal? TongTienThanhToanQuyDoi { get; set; }

        public bool? IsVND { get; set; }

        public string SoTienBangChu { get; set; }

        public int Row { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public bool? IsMainError { get; set; }

        public object Clone()
        {
            var obj = (HoaDonDienTuImport)MemberwiseClone();
            obj.HoaDonChiTiet = (HoaDonDienTuChiTietViewModel)HoaDonChiTiet.Clone();
            return obj;
        }
    }
}
