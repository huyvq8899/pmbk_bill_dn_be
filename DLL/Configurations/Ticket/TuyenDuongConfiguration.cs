using DLL.Entity.Ticket;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations.Ticket
{
    class TuyenDuongConfiguration : DbEntityConfiguration<TuyenDuong>
    {
        public override void Configure(EntityTypeBuilder<TuyenDuong> entity)
        {
            entity.HasKey(c => new { c.TuyenDuongId });
            entity.Property(c => c.TuyenDuongId).HasMaxLength(36);
        }
    }
}
