using Cooking.Entities.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.RecipePersistence.StepOperationDocuments
{
    internal class StepOperationDocumentEntiryMap : IEntityTypeConfiguration<StepOperationDocument>
    {
        public void Configure(EntityTypeBuilder<StepOperationDocument> builder)
        {
            builder.ToTable("StepOperationDocuments");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Extension)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(10);

            builder.Property(_ => _.DocumentId)
               .IsRequired();

            builder.HasOne(_ => _.StepOperation)
               .WithMany(_ => _.StepOperationDocuments)
               .HasForeignKey(_ => _.StepOperationId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
