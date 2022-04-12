﻿using Cooking.Entities.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.RecipePersistence.Recipes
{
    public class RecipeEntityMap : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes")
                .HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.FoodName)
                .HasMaxLength(200);

            builder.Property(_ => _.MainDocumentId);

            builder.Property(_ => _.MainDocumentExtension)
            .HasMaxLength(10);

            builder.Property(_ => _.Duration)
                .IsRequired(false);

            builder.Property(_ => _.ForHowManyPeople);

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
