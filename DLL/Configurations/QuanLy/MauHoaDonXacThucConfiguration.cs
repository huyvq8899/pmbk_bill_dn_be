using DLL.Entity.QuanLy;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLy
{
    public class MauHoaDonXacThucConfiguration : DbEntityConfiguration<MauHoaDonXacThuc>
    {
        public override void Configure(EntityTypeBuilder<MauHoaDonXacThuc> entity)
        {
            entity.HasKey(c => new { c.MauHoaDonXacThucId });

            entity.HasOne(u => u.NhatKyXacThucBoKyHieu)
               .WithMany(s => s.MauHoaDonXacThucs)
               .HasForeignKey(sc => sc.NhatKyXacThucBoKyHieuId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
