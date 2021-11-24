using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations
{
    public class FileDataConfiguration : DbEntityConfiguration<FileData>
    {
        public override void Configure(EntityTypeBuilder<FileData> entity)
        {
            entity.HasKey(c => new { c.FileDataId });
            entity.Property(c => c.FileDataId).HasMaxLength(36);
        }
    }
}
