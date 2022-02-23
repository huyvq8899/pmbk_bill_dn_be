using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class DangKyUyNhiemConfiguration : DbEntityConfiguration<DangKyUyNhiem>
    {
        public override void Configure(EntityTypeBuilder<DangKyUyNhiem> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.HasKey(c => new { c.Id });
        }
    }
}
