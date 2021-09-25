using Cooking.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.Documents
{
    internal class DocumentEntityMap : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.Property(_ => _.FileId)
                .IsRequired();

            builder.Property(_ => _.FileExtension)
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}