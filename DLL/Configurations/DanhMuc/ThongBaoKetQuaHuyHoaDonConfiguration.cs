using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonConfiguration : DbEntityConfiguration<ThongBaoKetQuaHuyHoaDon>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoKetQuaHuyHoaDon> entity)
        {
            entity.HasKey(c => new { c.ThongBaoKetQuaHuyHoaDonId });
        }
    }
}
