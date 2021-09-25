using Cooking.Entities.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.StatePersistence.Provinces
{
    internal class ProvinceEntityMap : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.Property(_ => _.Title)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();
        }
    }
}