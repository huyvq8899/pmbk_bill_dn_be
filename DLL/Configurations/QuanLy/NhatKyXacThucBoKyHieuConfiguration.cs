using DLL.Entity.QuanLy;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLy
{
    public class NhatKyXacThucBoKyHieuConfiguration : DbEntityConfiguration<NhatKyXacThucBoKyHieu>
    {
        public override void Configure(EntityTypeBuilder<NhatKyXacThucBoKyHieu> entity)
        {
            entity.HasKey(c => new { c.NhatKyXacThucBoKyHieuId });

            entity.HasOne(u => u.BoKyHieuHoaDon)
               .WithMany(s => s.NhatKyXacThucBoKyHieus)
               .HasForeignKey(sc => sc.BoKyHieuHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
