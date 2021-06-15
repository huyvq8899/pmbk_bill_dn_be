using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class LoaiTienConfiguration : DbEntityConfiguration<LoaiTien>
    {
        public override void Configure(EntityTypeBuilder<LoaiTien> entity)
        {
            entity.HasKey(c => new { c.LoaiTienId });
        }
    }
}
