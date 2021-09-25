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


            builder.Ignore(e => e.EmailConfirmed)
                .Ignore(e => e.NormalizedEmail)
                .Ignore(e => e.PhoneNumber)
                .Ignore(e => e.PhoneNumberConfirmed);
            builder.Property(_ => _.UserName)
                .HasMaxLength(50);

            builder.Property(_ => _.NormalizedUserName)
                .HasMaxLength(50);

            builder.Property(_ => _.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(_ => _.LastName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(_ => _.FatherName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(_ => _.NationalCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(_ => _.Email)
                .IsRequired(false);

            builder.Property(_ => _.CreationDate);

            builder.OwnsOne(_ => _.Mobile, _ =>
            {
                _.Property(_ => _.CountryCallingCode);
                _.Property(_ => _.MobileNumber);
            });

        }
    }
}