using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cooking.RestApi.Configs
{
    internal class CorsConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddCors();
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseCors(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        }
    }
}