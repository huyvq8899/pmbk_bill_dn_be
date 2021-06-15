using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class DonViTinhConfiguration : DbEntityConfiguration<DonViTinh>
    {
        public override void Configure(EntityTypeBuilder<DonViTinh> entity)
        {
            entity.HasKey(c => new { c.DonViTinhId });
        }
    }
}
