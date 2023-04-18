using DLL.Entity.QuanLyHoaDon;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class NhatKyThaoTacHoaDonConfiguration : DbEntityConfiguration<NhatKyThaoTacHoaDon>
    {
        public override void Configure(EntityTypeBuilder<NhatKyThaoTacHoaDon> entity)
        {
            entity.HasOne(u => u.HoaDonDienTu)
              .WithMany(s => s.NhatKyThaoTacHoaDons)
              .HasForeignKey(sc => sc.HoaDonDienTuId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
