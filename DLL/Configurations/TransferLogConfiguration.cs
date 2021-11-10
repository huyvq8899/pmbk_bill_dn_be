using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class TransferLogConfiguration : DbEntityConfiguration<TransferLog>
    {
        public override void Configure(EntityTypeBuilder<TransferLog> entity)
        {
            entity.HasKey(c => new { c.TransferLogId });
            entity.Property(c => c.TransferLogId).HasMaxLength(36);
        }
    }
}
