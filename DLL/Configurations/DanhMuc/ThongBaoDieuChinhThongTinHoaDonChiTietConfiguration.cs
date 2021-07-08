using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonChiTietConfiguration : DbEntityConfiguration<ThongBaoDieuChinhThongTinHoaDonChiTiet>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoDieuChinhThongTinHoaDonChiTiet> entity)
        {
            entity.HasKey(c => new { c.ThongBaoDieuChinhThongTinHoaDonId });

            entity.HasOne(u => u.ThongBaoDieuChinhThongTinHoaDon)
               .WithMany(s => s.ThongBaoDieuChinhThongTinHoaDonChiTiets)
               .HasForeignKey(sc => sc.ThongBaoDieuChinhThongTinHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.MauHoaDon)
               .WithMany(s => s.ThongBaoDieuChinhThongTinHoaDonChiTiets)
               .HasForeignKey(sc => sc.MauHoaDonId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
