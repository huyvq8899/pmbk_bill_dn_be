using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class DoiTuongConfiguration : DbEntityConfiguration<DoiTuong>
    {
        public override void Configure(EntityTypeBuilder<DoiTuong> entity)
        {
            entity.HasKey(c => new { c.DoiTuongId });
        }
    }
}
