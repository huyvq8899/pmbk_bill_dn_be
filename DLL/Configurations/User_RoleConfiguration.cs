using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class User_RoleConfiguration : DbEntityConfiguration<User_Role>
    {
        public override void Configure(EntityTypeBuilder<User_Role> entity)
        {
            entity.HasKey(c => new { c.URID });
            entity.Property(c => c.URID).HasMaxLength(36);

            entity.HasOne<User>(u => u.User)
            .WithMany(s => s.User_Roles)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Role>(u => u.Role)
            .WithMany(s => s.User_Roles)
            .HasForeignKey(sc => sc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
