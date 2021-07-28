using DLL.Entity.DanhMuc;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu2Configuration : DbEntityConfiguration<QuyetDinhApDungHoaDonDieu2>
    {
        public override void Configure(EntityTypeBuilder<QuyetDinhApDungHoaDonDieu2> entity)
        {
            entity.HasKey(c => new { c.QuyetDinhApDungHoaDonDieu2Id });

            entity.HasOne(u => u.QuyetDinhApDungHoaDon)
               .WithMany(s => s.QuyetDinhApDungHoaDonDieu2s)
               .HasForeignKey(sc => sc.QuyetDinhApDungHoaDonId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
