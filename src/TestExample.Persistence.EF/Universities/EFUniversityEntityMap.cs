using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestExample.Entities.Universities;

namespace TestExample.Persistence.EF.Universities
{
    public class EFUniversityEntityMap : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.ToTable("Universities");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(_ => _.Address).IsRequired();
            builder.Property(_ => _.Email).IsRequired();
            builder.Property(_ => _.Name).IsRequired();
        }
    }
}