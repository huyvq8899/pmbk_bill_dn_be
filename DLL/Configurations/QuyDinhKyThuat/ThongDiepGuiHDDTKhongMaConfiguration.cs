using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaConfiguration : DbEntityConfiguration<ThongDiepGuiHDDTKhongMa>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepGuiHDDTKhongMa> entity)
        {
            entity.HasKey(c => new { c.ThongDiepGuiHDDTKhongMaId });

            entity.HasOne(u => u.ThongDiepGuiHDDTKhongMaByte)
                 .WithOne(s => s.ThongDiepGuiHDDTKhongMa)
                 .HasForeignKey<ThongDiepGuiHDDTKhongMaByte>(x => x.ThongDiepGuiHDDTKhongMaId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
