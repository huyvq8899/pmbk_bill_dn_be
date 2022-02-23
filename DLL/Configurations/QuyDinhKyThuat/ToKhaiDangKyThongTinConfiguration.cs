using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ToKhaiDangKyThongTinConfigurationn : DbEntityConfiguration<ToKhaiDangKyThongTin>
    {
        public override void Configure(EntityTypeBuilder<ToKhaiDangKyThongTin> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.HasKey(c => c.Id);
        }
    }
}
