using Cooking.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.Documents
{
    internal class DocumentEntityMap : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> _)
        {
            _.ToTable("Documents");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedNever();

            _.Property(_ => _.Data).IsRequired();
            _.Property(_ => _.FileName).HasMaxLength(50).IsRequired();
            _.Property(_ => _.Extension).HasMaxLength(10).IsRequired();
            _.Property(_ => _.Status).IsRequired();
            _.Property(_ => _.CreationDate).IsRequired();
        }
    }
}