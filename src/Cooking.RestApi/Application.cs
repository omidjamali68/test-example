using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cooking.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cooking.RestApi
{
    internal class Application
    {
        private static Configuration[] _configurations;

        private static void Main(string[] args)
        {
            var baseDirectory = Directory.GetCurrentDirectory();
            var appSettings = ReadAppSettings(args, baseDirectory);
            var environment = appSettings.GetValue("environment", "Development");
            _configurations =
                GetConfiguratorsFromAssembly(typeof(Application).Assembly, args, appSettings, baseDirectory);

            InitializeConfigurators();

            var host = new WebHostBuilder()
                .UseContentRoot(baseDirectory)
                .UseEnvironment(environment)
                .ConfigureAppConfiguration(ConfigOptions)
                .ConfigureLogging(ConfigLogging)
                .UseStartup<Application>();

            ConfigServer(host);

            host.Build().Run();
        }

        public IServiceProvider ConfigureServices(IServiceCollection serviceContainer)
        {
            var container = new ContainerBuilder();

            _configurations.ForEach(c =>
            {
                c.ConfigureServiceContainer(serviceContainer);
                c.ConfigureServiceContainer(container);
            });

            container.Populate(serviceContainer);

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            _configurations.ForEach(c => c.ConfigureApplication(appBuilder));
        }

        private static IConfiguration ReadAppSettings(string[] args, string baseDirectory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile(Path.Combine(baseDirectory, "..", "config", "appsettings.json"), true, true)
                .AddJsonFile(Path.Combine(baseDirectory, "appsettings.json"), true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }

        private static Configuration[] GetConfiguratorsFromAssembly(
            Assembly assembly,
            string[] args,
            IConfiguration appSettings,
            string baseDirectory)
        {
            void SetPropertyValue<T>(object obj, string name, object value)
            {
                typeof(T).GetProperty(name)?.SetValue(obj, value);
            }

            return assembly.GetTypes()
                .Where(_ => _.IsAbstract == false)
                .Where(typeof(Configuration).IsAssignableFrom)
                .Select(configuratorType => new
                {
                    Type = configuratorType,
                    Config = configuratorType.GetCustomAttribute<ConfigurationAttribute>()
                })
                .Where(_ => _.Config?.Disabled != true)
                .OrderBy(_ => _.Config?.Order ?? 0)
                .Select(_ =>
                {
                    var configurator = Activator.CreateInstance(_.Type) as Configuration;
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.CommandLineArgs), args);
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.BaseDirectory), baseDirectory);
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.AppSettings), appSettings);
                    return configurator;
                })
                .ToArray();
        }

        private static void InitializeConfigurators()
        {
            _configurations.ForEach(_ => _.Initialized());
        }

        private static void ConfigOptions(IConfigurationBuilder builder)
        {
            _configurations.ForEach(configurator => configurator.ConfigureSettings(builder));
        }

        private static void ConfigLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
            _configurations.ForEach(configurator => configurator.ConfigureLogging(context, logging));
        }

        private static void ConfigServer(IWebHostBuilder hostBuilder)
        {
            _configurations.ForEach(_ => _.ConfigureServer(hostBuilder));
        }
    }
}