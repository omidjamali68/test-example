using Cooking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.RecipeSteps
{
    public class RecipeStepEntityMap : IEntityTypeConfiguration<RecipeStep>
    {
        public void Configure(EntityTypeBuilder<RecipeStep> builder)
        {
            builder.ToTable("RecipeSteps");

            builder.HasKey(_ => new { _.RecipeId, _.StepOperationId });
            
            builder.Property(_ => _.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(_ => _.Order)
                .IsRequired();
        }
    }
}
