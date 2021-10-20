using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTChiTietConfiguration : DbEntityConfiguration<DuLieuGuiHDDTChiTiet>
    {
        public override void Configure(EntityTypeBuilder<DuLieuGuiHDDTChiTiet> entity)
        {
            entity.HasKey(c => new { c.DuLieuGuiHDDTChiTietId });

            entity.HasOne(u => u.DuLieuGuiHDDT)
              .WithMany(s => s.DuLieuGuiHDDTChiTiets)
              .HasForeignKey(sc => sc.DuLieuGuiHDDTId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
