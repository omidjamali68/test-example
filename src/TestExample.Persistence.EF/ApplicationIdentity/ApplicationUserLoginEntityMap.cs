﻿using TestExample.Entities.ApplicationIdentities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestExample.Persistence.EF.ApplicationIdentity
{
    internal class ApplicationUserLoginEntityMap : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable("ApplicationUserLogins");
        }
    }
}