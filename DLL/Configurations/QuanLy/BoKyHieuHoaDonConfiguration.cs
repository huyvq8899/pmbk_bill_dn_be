﻿using DLL.Entity.QuanLy;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLy
{
    public class BoKyHieuHoaDonConfiguration : DbEntityConfiguration<BoKyHieuHoaDon>
    {
        public override void Configure(EntityTypeBuilder<BoKyHieuHoaDon> entity)
        {
            entity.HasKey(c => new { c.BoKyHieuHoaDonId });

            entity.HasOne(u => u.MauHoaDon)
               .WithMany(s => s.BoKyHieuHoaDons)
               .HasForeignKey(sc => sc.MauHoaDonId)
               .OnDelete(DeleteBehavior.Restrict);

            //entity.HasOne(u => u.ThongDiepChung)
            //   .WithMany(s => s.BoKyHieuHoaDons)
            //   .HasForeignKey(sc => sc.ThongDiepId)
            //   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
