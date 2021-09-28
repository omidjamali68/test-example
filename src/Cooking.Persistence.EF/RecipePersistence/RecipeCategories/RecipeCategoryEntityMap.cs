using Cooking.Entities.Recipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.RecipePersistence.RecipeCategories
{
    public class RecipeCategoryEntityMap : IEntityTypeConfiguration<RecipeCategory>
    {
        public void Configure(EntityTypeBuilder<RecipeCategory> builder)
        {
            builder.ToTable("RecipeCategories")
                .HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.Title)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
