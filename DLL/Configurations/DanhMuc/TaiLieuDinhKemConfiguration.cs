using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class TaiLieuDinhKemConfiguration : DbEntityConfiguration<TaiLieuDinhKem>
    {
        public override void Configure(EntityTypeBuilder<TaiLieuDinhKem> entity)
        {
            entity.HasKey(c => new { c.TaiLieuDinhKemId });
        }
    }
}
