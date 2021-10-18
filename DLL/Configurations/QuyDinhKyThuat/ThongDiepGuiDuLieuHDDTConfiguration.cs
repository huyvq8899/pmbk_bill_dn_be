using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTConfiguration : DbEntityConfiguration<ThongDiepGuiDuLieuHDDT>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepGuiDuLieuHDDT> entity)
        {
            entity.HasKey(c => new { c.ThongDiepGuiDuLieuHDDTId });
        }
    }
}
