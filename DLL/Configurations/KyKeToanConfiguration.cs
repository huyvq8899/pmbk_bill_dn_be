using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class KyKeToanConfiguration : DbEntityConfiguration<KyKeToan>
    {
        public override void Configure(EntityTypeBuilder<KyKeToan> entity)
        {
            entity.HasKey(c => new { c.KyKeToanId });
            entity.Property(c => c.KyKeToanId).HasMaxLength(36);
            //entity.Property<string>("CreatedDate"); // shadow property
        }
    }
}
