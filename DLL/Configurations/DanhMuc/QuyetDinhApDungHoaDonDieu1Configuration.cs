using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu1Configuration : DbEntityConfiguration<QuyetDinhApDungHoaDonDieu1>
    {
        public override void Configure(EntityTypeBuilder<QuyetDinhApDungHoaDonDieu1> entity)
        {
            entity.HasKey(c => new { c.QuyetDinhApDungHoaDonDieu1Id });

            entity.HasOne(u => u.QuyetDinhApDungHoaDon)
               .WithMany(s => s.QuyetDinhApDungHoaDonDieu1s)
               .HasForeignKey(sc => sc.QuyetDinhApDungHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
