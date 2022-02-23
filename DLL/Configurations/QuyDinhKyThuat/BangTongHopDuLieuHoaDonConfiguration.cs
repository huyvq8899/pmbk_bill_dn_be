using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDonConfiguration : DbEntityConfiguration<BangTongHopDuLieuHoaDon>
    {
        public override void Configure(EntityTypeBuilder<BangTongHopDuLieuHoaDon> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.HasKey(c => new { c.Id });
        }
    }
}
