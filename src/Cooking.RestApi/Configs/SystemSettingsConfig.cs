using Cooking.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cooking.RestApi.Configs
{
    public class SystemSettingsConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.Configure<SmsSettings>(options => AppSettings.GetSection("SmsSettings").Bind(options));
        }
    }
}