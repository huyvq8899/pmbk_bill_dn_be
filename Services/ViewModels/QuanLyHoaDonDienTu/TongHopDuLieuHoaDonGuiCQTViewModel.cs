using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class TongHopDuLieuHoaDonGuiCQTViewModel
    {
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public long? SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string MaSoThue { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public decimal? SoLuong { get; set; }
        public string DonViTinh { get; set; }
        public decimal? ThanhTien { get; set; }
        public string ThueGTGT { get; set; }
        public decimal? TienThueGTGT { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public string TenTrangThaiHoaDon { get; set; }
        public string MauSoHoaDonLienQuan { get; set; }
        public string KyHieuHoaDonLienQuan { get; set; }
        public string SoHoaDonLienQuan { get; set; }
    }
}
