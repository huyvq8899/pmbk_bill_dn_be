using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Config
{
    public class ThongTinPhatHanhConfiguration : DbEntityConfiguration<ThongTinPhatHanh>
    {
        public override void Configure(EntityTypeBuilder<ThongTinPhatHanh> entity)
        {
            entity.HasKey(c => new { c.Id });

            entity.Property(c => c.MaSoThue).HasMaxLength(16).IsUnicode(false);
        }
    }
}
