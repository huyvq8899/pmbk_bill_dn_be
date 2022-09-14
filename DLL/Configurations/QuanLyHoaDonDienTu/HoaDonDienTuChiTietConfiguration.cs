using DLL.Entity.QuanLyHoaDon;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuChiTietConfiguration : DbEntityConfiguration<HoaDonDienTuChiTiet>
    {
        public override void Configure(EntityTypeBuilder<HoaDonDienTuChiTiet> entity)
        {
            entity.Property(r => r.SoLuong).HasColumnType("decimal(21,6)");
            entity.Property(r => r.SoLuongNhap).HasColumnType("decimal(21,6)");
            entity.Property(r => r.DonGia).HasColumnType("decimal(21,6)");
            entity.Property(r => r.DonGiaSauThue).HasColumnType("decimal(21,6)");
            entity.Property(r => r.DonGiaQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.ThanhTien).HasColumnType("decimal(21,6)");
            entity.Property(r => r.ThanhTienQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.ThanhTienSauThue).HasColumnType("decimal(21,6)");
            entity.Property(r => r.ThanhTienSauThueQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TyLeChietKhau).HasColumnType("decimal(6,4)");
            entity.Property(r => r.TienChietKhau).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TienChietKhauQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TienThueGTGT).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TienThueGTGTQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThanhToan).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThanhToanQuyDoi).HasColumnType("decimal(21,6)");

            entity.Property(r => r.TyLePhanTramDoanhThu).HasColumnType("decimal(6,4)");
            entity.Property(r => r.TienGiam).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TienGiamQuyDoi).HasColumnType("decimal(21,6)");

            entity.Property(c => c.TenDonViTinh).HasMaxLength(50);
        }
    }
}
