using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Config
{
    public class TuyChonConfiguration : DbEntityConfiguration<TuyChon>
    {
        public override void Configure(EntityTypeBuilder<TuyChon> entity)
        {
            entity.HasKey(c => new { c.Ma });
        }
    }
}
