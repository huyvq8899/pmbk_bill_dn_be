using DLL.Entity.QuanLyHoaDon;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class ThongBaoChiTietHoaDonRaSoatConfiguration : DbEntityConfiguration<ThongBaoChiTietHoaDonRaSoat>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoChiTietHoaDonRaSoat> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);

            entity.Property(c => c.ThongBaoHoaDonRaSoatId).HasMaxLength(36);
            entity.Property(c => c.MauHoaDon).HasMaxLength(15);
            entity.Property(c => c.KyHieuHoaDon).HasMaxLength(10);
            entity.Property(c => c.SoHoaDon).HasMaxLength(10);
            entity.Property(c => c.LyDoRaSoat).HasMaxLength(300);
            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);
        }
    }
}
