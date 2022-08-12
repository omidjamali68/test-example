using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestExample.Entities.Masters;

namespace TestExample.Persistence.EF.Masters
{
    public class EFMasterEntityMap : IEntityTypeConfiguration<Master>
    {
        public void Configure(EntityTypeBuilder<Master> builder)
        {
            builder.ToTable("Masters");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Property(_ => _.FirstName).IsRequired();
            builder.Property(_ => _.LastName).IsRequired();
            builder.Property(_ => _.NationalCode).IsRequired();
            builder.Property(_ => _.Mobile).IsRequired();

            builder.HasOne(_ => _.University).WithMany(_ => _.Masters)
                .HasForeignKey(_ => _.UniversityId);
        }
    }
}