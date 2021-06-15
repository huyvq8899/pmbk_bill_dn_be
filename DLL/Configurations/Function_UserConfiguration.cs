using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class Function_UserConfiguration : DbEntityConfiguration<Function_User>
    {
        public override void Configure(EntityTypeBuilder<Function_User> entity)
        {
            entity.HasKey(c => new { c.FUID });
            entity.Property(c => c.FUID).HasMaxLength(36);

            entity.HasOne<Function>(u => u.Function)
            .WithMany(s => s.Function_Users)
            .HasForeignKey(sc => sc.FunctionId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Permission>(u => u.Permission)
            .WithMany(s => s.Function_Users)
            .HasForeignKey(sc => sc.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<User>(u => u.User)
            .WithMany(s => s.Function_Users)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
