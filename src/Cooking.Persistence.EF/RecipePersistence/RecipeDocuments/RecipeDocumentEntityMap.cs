using Cooking.Entities.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.RecipePersistence.RecipeDocuments
{
    public class RecipeDocumentEntityMap : IEntityTypeConfiguration<RecipeDocument>
    {
        public void Configure(EntityTypeBuilder<RecipeDocument> builder)
        {
            builder.ToTable("RecipeDocuments");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Extension)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(10);

            builder.Property(_ => _.DocumentId)
               .IsRequired();

            builder.HasOne(_ => _.Recipe)
               .WithMany(_ => _.RecipeDocuments)
               .HasForeignKey(_ => _.RecipeId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
