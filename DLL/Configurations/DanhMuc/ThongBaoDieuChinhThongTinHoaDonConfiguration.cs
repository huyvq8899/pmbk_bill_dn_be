using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonConfiguration : DbEntityConfiguration<ThongBaoDieuChinhThongTinHoaDon>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoDieuChinhThongTinHoaDon> entity)
        {
            entity.HasKey(c => new { c.ThongBaoDieuChinhThongTinHoaDonId });
        }
    }
}
