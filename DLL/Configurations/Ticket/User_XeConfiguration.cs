using DLL.Entity.Ticket;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.Ticket
{
    public class User_XeConfiguration : DbEntityConfiguration<User_Xe>
    {
        public override void Configure(EntityTypeBuilder<User_Xe> entity)
        {
            entity.HasKey(c => new { c.UserId, c.XeId });
            entity.Property(c => c.UserId).HasMaxLength(36);
            entity.Property(c => c.XeId).HasMaxLength(36);

            entity.HasOne(u => u.User)
             .WithMany(s => s.User_Xes)
             .HasForeignKey(sc => sc.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Xe)
             .WithMany(s => s.User_Xes)
             .HasForeignKey(sc => sc.XeId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
