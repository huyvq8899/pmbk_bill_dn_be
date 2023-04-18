using DLL.Entity.QuanLy;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLy
{
    public class BoKyHieuDangPhatHanhConfiguration : DbEntityConfiguration<BoKyHieuDangPhatHanh>
    {
        public override void Configure(EntityTypeBuilder<BoKyHieuDangPhatHanh> entity)
        {
            entity.HasKey(c => new { c.BoKyHieuHoaDonId });
            entity.Property(c => c.BoKyHieuHoaDonId).HasMaxLength(36);
        }
    }
}
