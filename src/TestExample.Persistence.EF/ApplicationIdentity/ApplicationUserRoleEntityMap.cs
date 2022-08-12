using TestExample.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestExample.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationUserRoleEntityMap : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("ApplicationUserRoles");
        }
    }
}