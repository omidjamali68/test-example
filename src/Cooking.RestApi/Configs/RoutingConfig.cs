using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cooking.RestApi.Configs
{
    internal class RoutingConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            //container.AddRouting();
            //container.AddControllers();


            container.AddRouting();
            container.AddControllers();
            container.AddHttpContextAccessor();

            container.AddMvcCore()
                .AddAuthorization(options =>
                    options.AddPolicy("Admin", policy =>
                        policy.RequireAuthenticatedUser()
                            .RequireRole("Admin")));


            /*container.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = AppSettings.GetValue<string>("auth:url");
                    options.RequireHttpsMetadata = AppSettings.GetValue<bool>("auth:httpsMetadata");
                    options.Audience = AppSettings.GetValue<string>("auth:apiName");
                });*/
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            //app.UseRouting();
            //app.UseEndpoints(endpoints => endpoints.MapControllers());


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}