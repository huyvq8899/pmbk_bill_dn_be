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

            entity.HasOne(s => s.MauHoaDon)
               .WithOne(sc => sc.MauHoaDonThietLapMacDinh)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
