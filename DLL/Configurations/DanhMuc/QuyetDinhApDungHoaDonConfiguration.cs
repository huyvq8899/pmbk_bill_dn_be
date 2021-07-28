using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class QuyetDinhApDungHoaDonConfiguration : DbEntityConfiguration<QuyetDinhApDungHoaDon>
    {
        public override void Configure(EntityTypeBuilder<QuyetDinhApDungHoaDon> entity)
        {
            entity.HasKey(c => new { c.QuyetDinhApDungHoaDonId });
        }
    }
}
