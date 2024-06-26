﻿using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Config
{
    public class ThietLapTruongDuLieuConfiguration : DbEntityConfiguration<ThietLapTruongDuLieu>
    {
        public override void Configure(EntityTypeBuilder<ThietLapTruongDuLieu> entity)
        {
            entity.HasKey(c => new { c.ThietLapTruongDuLieuId });

            entity.HasOne(u => u.BoKyHieuHoaDon)
               .WithMany(s => s.ThietLapTruongDuLieus)
               .HasForeignKey(sc => sc.BoKyHieuHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
