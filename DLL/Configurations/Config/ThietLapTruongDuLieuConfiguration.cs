using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Config
{
    public class ThietLapTruongDuLieuConfiguration : DbEntityConfiguration<ThietLapTruongDuLieu>
    {
        public override void Configure(EntityTypeBuilder<ThietLapTruongDuLieu> entity)
        {
            entity.HasKey(c => new { c.ThietLapTruongDuLieuId });

            //var model = new ThietLapTruongDuLieu();
            //entity.HasData(model.InitData());
        }
    }
}
