﻿using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class ThongBaoHoaDonRaSoatConfiguration : DbEntityConfiguration<ThongBaoHoaDonRaSoat>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoHoaDonRaSoat> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);

            entity.Property(c => c.MaThongDiep).HasMaxLength(50);
            entity.Property(c => c.SoThongBaoCuaCQT).HasMaxLength(40);
            entity.Property(c => c.TenCQTCapTren).HasMaxLength(120);
            entity.Property(c => c.TenCQTRaThongBao).HasMaxLength(120);

            entity.Property(c => c.TenNguoiNopThue).HasMaxLength(400);
            entity.Property(c => c.MaSoThue).HasMaxLength(15);
            entity.Property(c => c.HinhThuc).HasMaxLength(50);
            entity.Property(c => c.ChucDanh).HasMaxLength(50);
            entity.Property(c => c.FileDinhKem).HasMaxLength(255);

            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);
        }
    }
}
