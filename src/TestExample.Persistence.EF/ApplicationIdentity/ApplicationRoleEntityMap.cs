using TestExample.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestExample.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationRoleEntityMap : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("ApplicationRoles");
        }
    }
}