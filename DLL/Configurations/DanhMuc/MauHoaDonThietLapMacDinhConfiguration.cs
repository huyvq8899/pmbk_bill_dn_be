using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class MauHoaDonThietLapMacDinhConfiguration : DbEntityConfiguration<MauHoaDonThietLapMacDinh>
    {
        public override void Configure(EntityTypeBuilder<MauHoaDonThietLapMacDinh> entity)
        {
            entity.HasKey(c => new { c.MauHoaDonThietLapMacDinhId });

            entity.HasOne(u => u.MauHoaDon)
                .WithMany(s => s.MauHoaDonThietLapMacDinhs)
                .HasForeignKey(sc => sc.MauHoaDonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
