using DLL.Entity.QuanLy;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLy
{
    public class QuanLyThongTinHoaDonConfiguration : DbEntityConfiguration<QuanLyThongTinHoaDon>
    {
        public override void Configure(EntityTypeBuilder<QuanLyThongTinHoaDon> entity)
        {
            entity.HasKey(c => new { c.QuanLyThongTinHoaDonId });
            entity.Property(c => c.QuanLyThongTinHoaDonId).HasMaxLength(36);

            entity.Property(c => c.STT).IsRequired();
            entity.Property(c => c.LoaiThongTin).IsRequired();
            entity.Property(c => c.LoaiThongTinChiTiet).IsRequired();
            entity.Property(c => c.TrangThaiSuDung).IsRequired();
        }
    }
}
