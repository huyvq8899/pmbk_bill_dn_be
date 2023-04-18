using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class MauHoaDonConfiguration : DbEntityConfiguration<MauHoaDon>
    {
        public override void Configure(EntityTypeBuilder<MauHoaDon> entity)
        {
            entity.HasKey(c => new { c.MauHoaDonId });
        }
    }
}
