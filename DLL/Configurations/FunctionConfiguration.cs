using DLL.Entity;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(c => new { c.FunctionId });
            entity.Property(c => c.FunctionId).HasMaxLength(36);
        }
    }
}
