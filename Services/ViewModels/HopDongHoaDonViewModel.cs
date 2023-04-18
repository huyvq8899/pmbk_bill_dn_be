using DLL.Entity;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class HopDongHoaDonViewModel : ThongTinChung
    {
        public string HopDongHoaDonId { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime NgayDuyet { get; set; }
        public string MauHopDongId { get; set; }
        public string KhachHangId { get; set; }
        public string SoHopDong { get; set; }
        public string TenKhachHang { get; set; }
        public string NguoiDaiDien { get; set; }
        public string ChucVu { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string SoDienThoai { get; set; }
        public string NguoiLienHe { get; set; }
        public string SoDienThoaiNguoiLienHe { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHangMo { get; set; }
        public int GiaiDoan { get; set; } //0 - giai đoan 1, 1 - giai đoan 2,2-giai đoan 78

        //Thông tin phụ lục
        public string GoiBan { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DonGiaBan { get; set; }
        public decimal ThanhTienBan { get; set; }
        public int SoLuongTang { get; set; }
        public decimal DonGiaTang { get; set; }
        public decimal ThanhTienTang { get; set; }
        public int TongSoLuong { get; set; }
        public decimal TongThanhTien { get; set; }
        public decimal TienChietKhau { get; set; }
        public decimal TienThueGTGT { get; set; }
        public decimal TongThanhToan { get; set; }
        public string TongThanhToanBangChu { get; set; }
        public string SoftWareRef { get; set; }
        public string website { get; set; }
        public string KyHieu { get; set; }
        public string TuSo { get; set; }
        public string DenSo { get; set; }
        public string pathFilePDF { get; set; }
        public string GhiChu { get; set; }
        //Thông tin hỗ trợ
        public string NgayThiHanh { get; set; }
        public string TenCucThue { get; set; }
        public string CongTyPhatTrienKeToan { get; set; }
        public string LoaiHoaDon { get; set; }
        public string NoiLap { get; set; }
        public string LoaiUSB { get; set; }
        public string SoSeriUSB { get; set; }
        public string TuNgayUSB { get; set; }
        public string DenNgayUSB { get; set; }
        public string ChungThuSo { get; set; }
        public string MauSoHoaDon { get; set; }
        public string NganhKinhDoanh { get; set; }
        public string KyHieuFromDatabase { get; set; }
        public decimal TongTien { get; set; }

        public decimal PhiBanQuyen { get; set; }

        public decimal PhanMemKeToan { get; set; }
        public int MaxSoHoaDon { get; set; }
        public string MauSo { get; set; }
        public string NgayHoaDonCuoiCung { get; set; }
        public int TrangThaiHopDong { get; set; }

        public int LoaiCongTy { get; set; }

        public int TinhTrangHopDong { get; set; }

        public decimal TienPhiKhoiTao { get; set; }

        [MaxLength(4000)]
        public string GhiChuPhiKhoiTao { get; set; }

        [MaxLength(4000)]
        public string GhiChuSoLuongTang { get; set; }

        public int Thoigiansudung { get; set; }

        public decimal TongPhiBanQuyen { get; set; }

        [MaxLength(4000)]
        public string GhiChuPhanMemKeToan { get; set; }
        public string TenNguoiTao { get; set; }
        public string TenNguoiDuyet { get; set; }
        public string MstGp { get; set; }
        public bool? IsGiaHan { get; set; }
        public int LoaiHopDong { get; set; }

        public decimal TongPhiKeToan { get; set; }

        public decimal TongPhiKhoiTao { get; set; }
    }
}
