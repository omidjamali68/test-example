using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TestExample.RestApi.Configs
{
    internal class RoutingConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddRouting();
            container.AddControllers();
            container.AddHttpContextAccessor();

            container.AddMvcCore()
                .AddAuthorization(options =>
                    options.AddPolicy("Admin", policy =>
                        policy.RequireAuthenticatedUser()
                            .RequireRole("Admin")));
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}