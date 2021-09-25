using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Cooking.RestApi.Configs
{
    internal class LoggingConfig : Configuration
    {
        private const string LevelConfigKey = "Logging:Level";
        private const string FilePathKey = "Logging:FilePath";
        private bool enabled;
        private LogLevel level;
        private ILogger logger;

        public override void Initialized()
        {
            enabled = AppSettings.GetSection("Logging").Exists();
            if (enabled)
            {
                level = AppSettings.GetValue(LevelConfigKey, LogLevel.Warning);
                logger = CreateLogger(level, AppSettings.GetValue<string>(FilePathKey));
            }
        }

        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            if (enabled) container.AddSingleton(logger);
        }

        public override void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
            logging.ClearProviders();
            if (enabled)
            {
                logging.SetMinimumLevel(level)
                    .AddSerilog(logger);
                Log.Logger = logger;
            }
        }

        private ILogger CreateLogger(LogLevel minimumLevel, string? filePath = null)
        {
            var level = ToSerilogLogLevel(minimumLevel);

            var config = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .MinimumLevel.Override("Microsoft", level)
                .MinimumLevel.Override("System", level)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", level)
                .Enrich.FromLogContext()
                .WriteTo.Console(level);

            if (filePath != null)
                config = config.WriteTo.File(
                    filePath,
                    encoding: Encoding.UTF8,
                    restrictedToMinimumLevel: level);

            return config.CreateLogger();
        }

        private LogEventLevel ToSerilogLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Critical => LogEventLevel.Fatal,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Warning => LogEventLevel.Warning,
                _ => throw new InvalidCastException($"can't cast LogLevel '{logLevel}' to serilog log level")
            };
        }
    }
}