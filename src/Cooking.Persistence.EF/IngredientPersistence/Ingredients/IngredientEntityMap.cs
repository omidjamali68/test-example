using Cooking.Entities.Ingredients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.IngredientPersistence.Ingredients
{
    public class IngredientEntityMap : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients")
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

            builder.HasOne(_ => _.IngredientUnit)
                .WithMany(_ => _.Ingredients)
                .HasForeignKey(_ => _.IngredientUnitId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
