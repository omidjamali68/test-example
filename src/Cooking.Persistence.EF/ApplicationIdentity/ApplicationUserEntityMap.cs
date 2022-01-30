using Cooking.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationUserEntityMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.Property(_ => _.UserName)
                .HasMaxLength(100);

            builder.Property(_ => _.NormalizedUserName)
                .HasMaxLength(100);

            builder.Property(_ => _.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(_ => _.LastName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(_ => _.NationalCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(_ => _.Email)
                .IsRequired(false);

            builder.Property(_ => _.EmailConfirmed)
                .HasDefaultValue(false);

            builder.Property(_ => _.NormalizedEmail)
                .IsRequired(false);

            builder.Property(_ => _.PhoneNumber)
                .IsRequired(false);

            builder.Property(_ => _.PhoneNumberConfirmed)
                .HasDefaultValue(false);

            builder.Property(_ => _.CreationDate);

        }
    }
}