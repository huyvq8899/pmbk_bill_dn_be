using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class UserConfiguration : DbEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(c => new { c.UserId });
            entity.Property(c => c.UserId).HasMaxLength(36);

            entity.HasOne<Role>(u => u.Role)
           .WithMany(s => s.Users)
           .HasForeignKey(sc => sc.RoleId)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
