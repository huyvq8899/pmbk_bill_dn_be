using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonChiTietConfiguration : DbEntityConfiguration<ThongBaoKetQuaHuyHoaDonChiTiet>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoKetQuaHuyHoaDonChiTiet> entity)
        {
            entity.HasKey(c => new { c.ThongBaoKetQuaHuyHoaDonChiTietId });

            entity.HasOne(u => u.ThongBaoKetQuaHuyHoaDon)
               .WithMany(s => s.ThongBaoKetQuaHuyHoaDonChiTiets)
               .HasForeignKey(sc => sc.ThongBaoKetQuaHuyHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.MauHoaDon)
               .WithMany(s => s.ThongBaoKetQuaHuyHoaDonChiTiets)
               .HasForeignKey(sc => sc.MauHoaDonId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
