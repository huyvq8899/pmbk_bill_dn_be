using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTConfiguration : DbEntityConfiguration<DuLieuGuiHDDT>
    {
        public override void Configure(EntityTypeBuilder<DuLieuGuiHDDT> entity)
        {
            entity.HasKey(c => new { c.DuLieuGuiHDDTId });
        }
    }
}
