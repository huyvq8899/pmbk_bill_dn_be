using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTChiTietConfiguration : DbEntityConfiguration<ThongDiepGuiDuLieuHDDTChiTiet>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepGuiDuLieuHDDTChiTiet> entity)
        {
            entity.HasKey(c => new { c.ThongDiepGuiDuLieuHDDTChiTietId });

            entity.HasOne(u => u.ThongDiepGuiDuLieuHDDT)
              .WithMany(s => s.ThongDiepGuiDuLieuHDDTChiTiets)
              .HasForeignKey(sc => sc.ThongDiepGuiDuLieuHDDTId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
