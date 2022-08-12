using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TestExample.RestApi.Configs
{
    internal class VersioningConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            container.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}