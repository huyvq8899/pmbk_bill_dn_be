using DLL.Entity.TienIch;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.TienIch
{
    public class NhatKyGuiEmailConfiguration : DbEntityConfiguration<NhatKyGuiEmail>
    {
        public override void Configure(EntityTypeBuilder<NhatKyGuiEmail> entity)
        {
            entity.HasKey(c => new { c.NhatKyGuiEmailId });
        }
    }
}
