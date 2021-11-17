using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.HoaDonDienTu
{
    public class ThongTinHoaDonConfiguration : DbEntityConfiguration<ThongTinHoaDon>
    {
        public override void Configure(EntityTypeBuilder<ThongTinHoaDon> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.Property(c => c.FileDinhKem).HasMaxLength(255);

            entity.Property(c => c.MaCQTCap).HasMaxLength(40);
            entity.Property(c => c.MauSoHoaDon).HasMaxLength(15);
            entity.Property(c => c.KyHieuHoaDon).HasMaxLength(10);
            entity.Property(c => c.SoHoaDon).HasMaxLength(10);
        }
    }

    public class ThongTinHoaDonBienBanXoaBoConfiguration : DbEntityConfiguration<BienBanXoaBo>
    {
        public override void Configure(EntityTypeBuilder<BienBanXoaBo> entity)
        {
            entity.Property(c => c.ThongTinHoaDonId).HasMaxLength(36);
        }
    }
}
