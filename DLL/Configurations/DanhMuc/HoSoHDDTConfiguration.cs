using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class HoSoHDDTConfiguration : DbEntityConfiguration<HoSoHDDT>
    {
        public override void Configure(EntityTypeBuilder<HoSoHDDT> entity)
        {
            entity.HasKey(c => new { c.HoSoHDDTId });
        }
    }
}
