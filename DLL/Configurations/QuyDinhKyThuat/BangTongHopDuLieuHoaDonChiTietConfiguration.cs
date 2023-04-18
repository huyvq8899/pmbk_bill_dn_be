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
            entity.Property(c => c.TTPhi).HasColumnType("decimal(7,2)");
            entity.Property(c => c.TGTKhac).HasColumnType("decimal(7,2)");
            entity.Property(c => c.TGia).HasColumnType("decimal(7,2)");
        }
    }
}
