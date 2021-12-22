using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class ThongDiepChiTietGuiCQTConfiguration : DbEntityConfiguration<ThongDiepChiTietGuiCQT>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepChiTietGuiCQT> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.Property(c => c.ThongDiepGuiCQTId).HasMaxLength(36);
            entity.Property(c => c.HoaDonDienTuId).HasMaxLength(36);
            entity.Property(c => c.ThongBaoChiTietHDRaSoatId).HasMaxLength(36);

            entity.Property(c => c.MaCQTCap).HasMaxLength(40);
            entity.Property(c => c.MauHoaDon).HasMaxLength(15);
            entity.Property(c => c.KyHieuHoaDon).HasMaxLength(10);
            entity.Property(c => c.SoHoaDon).HasMaxLength(10);
            entity.Property(c => c.LyDo).HasMaxLength(300);

            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);

            entity.Property(c => c.ChungTuLienQuan).HasMaxLength(40);
            entity.Property(c => c.DienGiaiTrangThai).HasMaxLength(50);
        }
    }
}
