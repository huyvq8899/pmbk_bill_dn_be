using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuConfiguration : DbEntityConfiguration<HoaDonDienTu>
    {
        public override void Configure(EntityTypeBuilder<HoaDonDienTu> entity)
        {
            entity.Property(c => c.LoaiChietKhau).HasDefaultValue(LoaiChietKhau.TheoMatHang);
        }
    }
}
