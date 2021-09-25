using Cooking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.StepOperations
{
    public class StepOperationEntityMap : IEntityTypeConfiguration<StepOperation>
    {
        public void Configure(EntityTypeBuilder<StepOperation> builder)
        {
            builder.ToTable("StepOperations")
                .HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(_ => _.AvatarId);
            builder.Property(_ => _.Extension)
               .IsUnicode()
               .HasMaxLength(10);
        }
    }
}
