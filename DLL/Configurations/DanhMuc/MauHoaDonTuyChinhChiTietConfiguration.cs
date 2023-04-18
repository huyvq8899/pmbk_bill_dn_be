using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class MauHoaDonTuyChinhChiTietConfiguration : DbEntityConfiguration<MauHoaDonTuyChinhChiTiet>
    {
        public override void Configure(EntityTypeBuilder<MauHoaDonTuyChinhChiTiet> entity)
        {
            entity.HasKey(c => new { c.MauHoaDonTuyChinhChiTietId });

            entity.HasOne(u => u.MauHoaDon)
                .WithMany(s => s.MauHoaDonTuyChinhChiTiets)
                .HasForeignKey(sc => sc.MauHoaDonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
