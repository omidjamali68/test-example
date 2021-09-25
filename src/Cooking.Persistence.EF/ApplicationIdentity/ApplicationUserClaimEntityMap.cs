using Cooking.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cooking.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationUserClaimEntityMap : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable("ApplicationUserClaims");
        }
    }
}