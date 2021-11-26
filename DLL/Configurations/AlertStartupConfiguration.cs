using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class AlertStartupConfiguration : DbEntityConfiguration<AlertStartup>
    {
        public override void Configure(EntityTypeBuilder<AlertStartup> entity)
        {
            entity.ToTable("AlertStartups");

            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);
        }
    }
}
