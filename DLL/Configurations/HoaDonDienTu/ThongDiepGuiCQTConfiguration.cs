using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.HoaDonDienTu
{
    public class ThongDiepGuiCQTConfiguration : DbEntityConfiguration<ThongDiepGuiCQT>
    {
        public override void Configure(EntityTypeBuilder<ThongDiepGuiCQT> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.Property(c => c.MaThongDiep).HasMaxLength(50);
            entity.Property(c => c.NguoiNopThue).HasMaxLength(50);
            entity.Property(c => c.DiaDanh).HasMaxLength(50);
            entity.Property(c => c.ThongBaoHoaDonRaSoatId).HasMaxLength(36);

            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);

            entity.Property(c => c.FileXMLDaKy).HasMaxLength(200);
            entity.Property(c => c.FileDinhKem).HasMaxLength(500);
        }
    }
}
