using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class HangHoaDichVuConfiguration : DbEntityConfiguration<HangHoaDichVu>
    {
        public override void Configure(EntityTypeBuilder<HangHoaDichVu> entity)
        {
            entity.HasKey(c => new { c.HangHoaDichVuId });

            entity.HasOne(u => u.DonViTinh)
               .WithMany(s => s.HangHoaDichVus)
               .HasForeignKey(sc => sc.DonViTinhId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
