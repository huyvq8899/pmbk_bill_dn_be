using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaDuLieuConfiguration : DbEntityConfiguration<ThongDiepGuiHDDTKhongMaDuLieu>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepGuiHDDTKhongMaDuLieu> entity)
        {
            entity.HasKey(c => new { c.ThongDiepGuiHDDTKhongMaDuLieuId });

            entity.HasOne(u => u.ThongDiepGuiHDDTKhongMa)
              .WithMany(s => s.ThongDiepGuiHDDTKhongMaDuLieus)
              .HasForeignKey(sc => sc.ThongDiepGuiHDDTKhongMaId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
