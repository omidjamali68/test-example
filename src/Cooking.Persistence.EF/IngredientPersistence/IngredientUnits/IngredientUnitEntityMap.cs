using Cooking.Entities.Ingredients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.IngredientPersistence.IngredientUnits
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
