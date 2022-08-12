using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Entities.Masters;
using TestExample.Entities.Universities;

namespace TestExample.Persistence.EF
{
    public class EFDataContext : IdentityDbContext<ApplicationUser
        , ApplicationRole
        , Guid
        , ApplicationUserClaim
        , ApplicationUserRole
        , ApplicationUserLogin
        , ApplicationRoleClaim
        , ApplicationUserToken>
    {
        public EFDataContext(string connectionString)
            : this(new DbContextOptionsBuilder<EFDataContext>().UseSqlServer(connectionString).Options)
        {
        }

        private EFDataContext(DbContextOptions<EFDataContext> options) : base(options)
        {
        }

        public DbSet<University> Universities { get; set; }
        public DbSet<Master> Masters { get; set; }

        public override ChangeTracker ChangeTracker
        {
            get
            {
                var tracker = base.ChangeTracker;
                tracker.LazyLoadingEnabled = false;
                tracker.AutoDetectChangesEnabled = true;
                tracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                return tracker;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFDataContext).Assembly);
        }
    }
}