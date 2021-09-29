using System;
using Cooking.Entities;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Entities.Recipes;
using Cooking.Entities.States;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cooking.Persistence.EF
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

        #region ApplicationIdentities

        //public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        //public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        //public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        //public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
        public DbSet<IdentityVerificationCode> VerificationCodes { get; set; }
        //public DbSet<UserTimeZone> UserTimeZones { get; set; }

        #endregion
        
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

        #region Documents

        public DbSet<Document> Documents { get; protected set; }

        #endregion

        #region States

        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }

        #endregion

        #region Ingredients

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientUnit> IngredientUnits { get; set; }

        #endregion

        #region Recepies

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<RecipeDocument> RecipeDocuments { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<StepOperation> StepOperations { get; set; }

        #endregion
        
    }
}