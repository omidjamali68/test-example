using Cooking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.Recipes
{
    public class RecipeEntityMap : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes")
                .HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.FoodName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(_ => _.Duration)
                .IsRequired(false);

            builder.HasOne(_ => _.RecipeCategory)
                .WithMany(_ => _.Recipes)
                .HasForeignKey(_ => _.RecipeCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(_ => _.Nationality)
                .WithMany(_ => _.Recipes)
                .HasForeignKey(_ => _.NationalityId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
