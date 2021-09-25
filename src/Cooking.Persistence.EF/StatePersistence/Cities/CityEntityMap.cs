using Cooking.Entities.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.StatePersistence.Cities
{
    internal class CityEntityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();

            builder.Property(_ => _.Title)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder.HasOne(_ => _.Province)
                .WithMany(_ => _.Cities)
                .HasForeignKey(_ => _.ProvinceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}