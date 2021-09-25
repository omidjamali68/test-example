using Cooking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.RecipeIngredients
{
    public class RecipeIngredientEntityMap : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            builder.HasKey(_ => new { _.RecipeId, _.IngredientId });
            builder.Property(_ => _.Quantity)
                .IsRequired();
        }
    }
}
