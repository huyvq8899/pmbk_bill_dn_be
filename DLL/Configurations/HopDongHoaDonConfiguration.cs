using DLL.Entity.DanhMuc;
using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.NghiepVu
{
    public class HopDongHoaDonConfiguration : DbEntityConfiguration<HopDongHoaDon>
    {
        public override void Configure(EntityTypeBuilder<HopDongHoaDon> entity)
        {
            entity.HasKey(c => new { c.HopDongHoaDonId });
            entity.Property(c => c.HopDongHoaDonId).HasMaxLength(36);
        }
    }
}
