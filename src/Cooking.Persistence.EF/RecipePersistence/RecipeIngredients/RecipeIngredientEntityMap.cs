using Cooking.Entities.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.RecipePersistence.RecipeIngredients
{
    public class RecipeIngredientEntityMap : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            builder.HasKey(_ => new { _.RecipeId, _.IngredientId });
            builder.Property(_ => _.Quantity)
                .IsRequired();

            builder.HasOne(_ => _.Recipe)
                .WithMany(_ => _.RecipeIngredients)
                .HasForeignKey(_ => _.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.Ingredient)
             .WithMany(_ => _.RecipeIngredients)
             .HasForeignKey(_ => _.IngredientId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
