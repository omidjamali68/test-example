using Cooking.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.ApplicationIdentity
{
    internal class IdentityVerificationCodeEntityMap : IEntityTypeConfiguration<IdentityVerificationCode>
    {
        public void Configure(EntityTypeBuilder<IdentityVerificationCode> builder)
        {
            builder.ToTable("VerificationCodes");

            builder.Property(_ => _.PhoneNumber);
        }
    }
}