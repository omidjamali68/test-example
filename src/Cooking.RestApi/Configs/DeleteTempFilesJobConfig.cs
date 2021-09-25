using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Infrastructure;
using Cooking.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cooking.RestApi.Configs
{
    internal class DeleteTempFilesJobConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.Configure<DeleteTempFilesJobOptions>(_ =>
            {
                _.JobStepDelay = TimeSpan.FromMinutes(10);
                _.ReserveFileDuration = TimeSpan.FromHours(1);
            });

            container.AddHostedService<DeleteTempFileJobService>();
        }
    }

    internal class DeleteTempFilesJobOptions
    {
        public DeleteTempFilesJobOptions()
        {
            SetDefaultValues();
        }

        public TimeSpan JobStepDelay { get; set; }
        public TimeSpan ReserveFileDuration { get; set; }

        private void SetDefaultValues()
        {
            JobStepDelay = TimeSpan.FromMinutes(10);
            ReserveFileDuration = TimeSpan.FromHours(1);
        }
    }

    internal class DeleteTempFileJobService : BackgroundService
    {
        private readonly DateTimeService _dateTimeService;
        private readonly ILogger<DeleteTempFileJobService> _logger;
        private readonly DeleteTempFilesJobOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public DeleteTempFileJobService(
            DateTimeService dateTimeService,
            IOptions<DeleteTempFilesJobOptions> options,
            ILogger<DeleteTempFileJobService> logger,
            IServiceProvider serviceProvider)
        {
            _dateTimeService = dateTimeService;
            _options = options.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<EFDataContext>();
                    await DoWork(dataContext, stoppingToken);
                }

                await Task.Delay(_options.JobStepDelay, stoppingToken);
            }
        }

        private async Task DoWork(EFDataContext dataContext, CancellationToken stoppingToken)
        {
            var thresholdTime = _dateTimeService.Now.Subtract(_options.ReserveFileDuration);
            var toDeletes = await dataContext.Documents.AsNoTracking().Where(_ =>
                    _.Status == DocumentStatus.Deleted ||
                    _.Status == DocumentStatus.Reserved && _.CreationDate < thresholdTime)
                .Select(_ => new
                {
                    DocumentId = _.Id, _.FileId
                })
                .ToListAsync(stoppingToken);

            if (toDeletes.Any() == false)
            {
                _logger.LogInformation("no documents were found to clean");
                return;
            }

            _logger.LogInformation($"{toDeletes.Count} documents were found to clean");

            toDeletes.Select(_ => new Document {Id = _.DocumentId}).ForEach(document =>
            {
                dataContext.Attach(document);
                dataContext.Entry(document).State = EntityState.Deleted;
            });

            _logger.LogInformation("documents cleaning...");

            try
            {
                await dataContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation($"{toDeletes.Count} documents cleaned successfully");
            }
            catch (Exception ex)
            {
                var documentIds = toDeletes.Select(_ => _.DocumentId);
                _logger.LogError(ex, $"documents were not cleaned. (keys: {string.Join(',', documentIds)})");
            }
        }
    }
}