using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoPhatHanhConfiguration : DbEntityConfiguration<ThongBaoPhatHanh>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoPhatHanh> entity)
        {
            entity.HasKey(c => new { c.ThongBaoPhatHanhId });
        }
    }
}
