using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Config
{
    public class TaiKhoanSmartCAConfiguration : DbEntityConfiguration<TaiKhoanSmartCA>
    {
        public override void Configure(EntityTypeBuilder<TaiKhoanSmartCA> entity)
        {
            entity.HasKey(c => new { c.TaiKhoanSmartCAId });
        }
    }
}
