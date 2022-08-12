using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Entities.CommonEntities;
using TestExample.Infrastructure.Application;
using TestExample.Persistence.EF;

namespace TestExample.RestApi.Configs
{
    public class SeedDataConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddSingleton<SeedDataService>();
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var seedDataService = app.ApplicationServices
            .GetRequiredService<SeedDataService>();
            _ = seedDataService.Initialize();
        }
    }

    public class SeedDataService
    {
        public IServiceProvider _serviceProvider;

        public SeedDataService(IServiceProvider service)
        {
            _serviceProvider = service;
        }

        public async Task Initialize()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = _serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var context = _serviceProvider.GetRequiredService<EFDataContext>();

                if (!context.Roles.Any())
                {
                    var roles = new List<string>
                    {
                        UserRoles.Admin
                    };

                    foreach (var role in roles)
                    {
                        var applicationRole = new ApplicationRole { Name = role };
                        await roleManager.CreateAsync(applicationRole);
                    }
                }

                if (!context.Set<ApplicationUser>().Any())
                {
                    var admin = GenerateAdmin();
                    var createdResult = await userManager.CreateAsync(
                        admin, "123456");
                    if (createdResult.Succeeded)
                        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
                }
            }
        }

        private static ApplicationUser GenerateAdmin()
        {
            return new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                CreationDate = DateTime.Now,
                Mobile = new Mobile
                {
                    CountryCallingCode = "0098",
                    MobileNumber = "9177870290"
                },
                NationalCode = "2280113732",
                UserName = "2280113732"
            };
        }
    }
}