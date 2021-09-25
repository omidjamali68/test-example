using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cooking.RestApi.Configs
{
    internal class VersioningConfig : Configuration
    {
        private ApiVersion _defaultVersion;
        private IApiVersionReader _versionReader;

        public override void Initialized()
        {
            _defaultVersion = ApiVersion.Parse("1.0");
            _versionReader = new MediaTypeApiVersionReader("version");
        }

        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = _defaultVersion;
                options.ApiVersionReader = _versionReader;
            });

            container.AddVersionedApiExplorer(options =>
            {
                var vOptions = container.BuildServiceProvider().GetService<IOptions<ApiVersioningOptions>>()?.Value;

                if (vOptions?.DefaultApiVersion != null) options.DefaultApiVersion = vOptions.DefaultApiVersion;

                options.GroupNameFormat = "'v'VVVV";
            });
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/_version", async context =>
                {
                    var version = AppSettings.GetValue<string>("Version")
                                  ?? ResolveVersionFromFile()
                                  ?? string.Empty;
                    context.Response.ContentType = "text/plain";
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync(version);
                });
                endpoints.MapControllers();
            });
        }

        private string? ResolveVersionFromFile()
        {
            var filePath = Path.Combine(BaseDirectory, "version");
            if (File.Exists(filePath))
                return File.ReadAllText(filePath).Trim();
            return null;
        }
    }
}