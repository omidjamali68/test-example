using Cooking.Entities.Recipe;
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
        }
    }
}
