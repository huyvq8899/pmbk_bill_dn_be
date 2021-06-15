using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class RoleConfiguration : DbEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.HasKey(c => new { c.RoleId });
            entity.Property(c => c.RoleId).HasMaxLength(36);
        }
    }
}
