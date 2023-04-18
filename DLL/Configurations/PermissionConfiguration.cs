using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class PermissionConfiguration : DbEntityConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> entity)
        {
            entity.HasKey(c => new { c.PermissionId });
            entity.Property(c => c.PermissionId).HasMaxLength(36);
        }
    }
}
