using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ThongDiepChungConfiguration : DbEntityConfiguration<ThongDiepChung>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepChung> entity)
        {
            entity.Property(c => c.SoTBaoPhanHoiCuaCQT).HasMaxLength(50);
            entity.Property(c => c.IdTDiepTBaoPhanHoiCuaCQT).HasMaxLength(36);
        }
    }
}
