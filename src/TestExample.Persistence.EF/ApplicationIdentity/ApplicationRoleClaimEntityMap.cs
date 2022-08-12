using TestExample.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestExample.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationRoleClaimEntityMap : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("ApplicationRoleClaims");
        }
    }
}