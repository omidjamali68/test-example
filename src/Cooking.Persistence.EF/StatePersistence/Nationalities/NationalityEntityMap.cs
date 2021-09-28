using Cooking.Entities.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.StatePersistence.Nationalities
{
    public class NationalityEntityMap : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> builder)
        {
            builder.ToTable("Nationalities")
                .HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
