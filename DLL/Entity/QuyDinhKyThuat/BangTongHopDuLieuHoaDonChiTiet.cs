﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDonChiTiet
    {
        public string Id { get; set; }

        public string BangTongHopDuLieuHoaDonId { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string MaSoThue { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal? SoLuong { get; set; }
        public string DonViTinh { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal? ThanhTien { get; set; }
        public string ThueGTGT { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal? TienThueGTGT { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal? TongTienThanhToan { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public string TenTrangThaiHoaDon { get; set; }
        public string MauSoHoaDonLienQuan { get; set; }
        public string KyHieuHoaDonLienQuan { get; set; }
        public string SoHoaDonLienQuan { get; set; }
    }
}
