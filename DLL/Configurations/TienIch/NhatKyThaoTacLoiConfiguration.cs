using DLL.Entity.TienIch;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.TienIch
{
    public class NhatKyThaoTacLoiConfiguration : DbEntityConfiguration<NhatKyThaoTacLoi>
    {
        public override void Configure(EntityTypeBuilder<NhatKyThaoTacLoi> entity)
        {
            entity.HasKey(c => new { c.NhatKyThaoTacLoiId });
            entity.Property(c => c.NhatKyThaoTacLoiId).HasMaxLength(36);

            entity.Property(c => c.MoTa).HasMaxLength(256);
            entity.Property(c => c.HuongDanXuLy).HasMaxLength(256);
            entity.Property(c => c.RefId).HasMaxLength(36);
        }
    }
}
