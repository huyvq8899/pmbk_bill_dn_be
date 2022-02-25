using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class MauHoaDonFileConfiguration : DbEntityConfiguration<MauHoaDonFile>
    {
        public override void Configure(EntityTypeBuilder<MauHoaDonFile> entity)
        {
            entity.HasKey(c => new { c.MauHoaDonFileId });

            entity.Property(c => c.MauHoaDonFileId).HasMaxLength(36);
            entity.Property(c => c.FileName).HasMaxLength(50);
            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);

            entity.HasOne(u => u.MauHoaDon)
                .WithMany(s => s.MauHoaDonFiles)
                .HasForeignKey(sc => sc.MauHoaDonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
