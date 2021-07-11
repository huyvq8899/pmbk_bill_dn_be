using DLL.Entity.TienIch;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.TienIch
{
    public class NhatKyTruyCapConfiguration : DbEntityConfiguration<NhatKyTruyCap>
    {
        public override void Configure(EntityTypeBuilder<NhatKyTruyCap> entity)
        {
            entity.HasKey(c => new { c.NhatKyTruyCapId });
        }
    }
}
