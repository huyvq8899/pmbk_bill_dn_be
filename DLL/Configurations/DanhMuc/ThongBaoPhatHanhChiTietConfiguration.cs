using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoPhatHanhChiTietConfiguration : DbEntityConfiguration<ThongBaoPhatHanhChiTiet>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoPhatHanhChiTiet> entity)
        {
            entity.HasKey(c => new { c.ThongBaoPhatHanhChiTietId });

            entity.HasOne(u => u.ThongBaoPhatHanh)
               .WithMany(s => s.ThongBaoPhatHanhChiTiets)
               .HasForeignKey(sc => sc.ThongBaoPhatHanhId)
               .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.MauHoaDon)
               .WithMany(s => s.ThongBaoPhatHanhChiTiets)
               .HasForeignKey(sc => sc.MauHoaDonId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
