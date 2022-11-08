using DLL.Entity.Ticket;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Ticket
{
    public class XeConfiguration : DbEntityConfiguration<Xe>
    {
        public override void Configure(EntityTypeBuilder<Xe> entity)
        {
            entity.HasKey(c => new { c.XeId });
            entity.Property(c => c.XeId).HasMaxLength(36);

            entity.Property(c => c.MaXe).HasMaxLength(256);
            entity.Property(c => c.SoXe).HasMaxLength(256);
            entity.Property(c => c.LoaiXe).HasMaxLength(256);
        }
    }
}
