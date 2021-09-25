using Cooking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.IngredientUnits
{
    public class IngredientUnitEntityMap : IEntityTypeConfiguration<IngredientUnit>
    {
        public void Configure(EntityTypeBuilder<IngredientUnit> builder)
        {
            builder.ToTable("IngredientUnits")
                .HasKey("Id");
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.Title)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
