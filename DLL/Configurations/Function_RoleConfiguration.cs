using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class Function_RoleConfiguration : DbEntityConfiguration<Function_Role>
    {
        public override void Configure(EntityTypeBuilder<Function_Role> entity)
        {
            entity.HasKey(c => new { c.FRID });
            entity.Property(c => c.FRID).HasMaxLength(36);

            entity.HasOne<Function>(u => u.Function)
            .WithMany(s => s.Function_Roles)
            .HasForeignKey(sc => sc.FunctionId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Permission>(u => u.Permission)
            .WithMany(s => s.Function_Roles)
            .HasForeignKey(sc => sc.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Role>(u => u.Role)
            .WithMany(s => s.Function_Roles)
            .HasForeignKey(sc => sc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
