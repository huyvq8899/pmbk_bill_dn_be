using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDonChiTietConfiguration : DbEntityConfiguration<BangTongHopDuLieuHoaDonChiTiet>
    {
        public override void Configure(EntityTypeBuilder<BangTongHopDuLieuHoaDonChiTiet> entity)
        {
            entity.HasKey(c => new { c.Id });

            entity.HasOne(u => u.BangTongHopDuLieuHoaDon)
              .WithMany(s => s.ChiTiets)
              .HasForeignKey(sc => sc.BangTongHopDuLieuHoaDonId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
