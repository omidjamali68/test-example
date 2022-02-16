using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure;
using Cooking.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cooking.RestApi.Configs
{
    class DeleteUnReferencedFilesJobConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.Configure<DeleteUnReferencedFilesJobOptions>(_ =>
            {
                _.JobStepDelay = TimeSpan.FromHours(1);
                _.ReserveFileDuration = TimeSpan.FromHours(1);
            });

            container.AddHostedService<DeleteUnReferencedFileJobService>();
        }
    }

    class DeleteUnReferencedFilesJobOptions
    {
        public TimeSpan JobStepDelay { get; set; }
        public TimeSpan ReserveFileDuration { get; set; }

        public DeleteUnReferencedFilesJobOptions()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            JobStepDelay = TimeSpan.FromHours(1);
            ReserveFileDuration = TimeSpan.FromHours(1);
        }
    }

    class DeleteUnReferencedFileJobService : BackgroundService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly DeleteUnReferencedFilesJobOptions _options;
        private readonly ILogger<DeleteUnReferencedFileJobService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DeleteUnReferencedFileJobService(
            IDateTimeService dateTimeService,
            IOptions<DeleteUnReferencedFilesJobOptions> options,
            ILogger<DeleteUnReferencedFileJobService> logger,
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
            var documentTypes = FindTypesWithDocument();

            var referencedDocumentIds = FindReferencedDocumentIds(dataContext, documentTypes);
            var allDocumentIds = GetAllDocumentIds(dataContext);
            var unReferencedDocumentIds = FindUnReferencedDocumentIds(allDocumentIds, referencedDocumentIds);

            await DeleteUnReferencedDocuments(dataContext, stoppingToken, unReferencedDocumentIds);
        }

        private List<DocumentTypeAndColumnDto> FindTypesWithDocument()
        {
            var assembly = Assembly.GetAssembly(typeof(RecipeDocument));

            var typeWithDocuments = assembly?.GetTypes()
                .Where(_ => _.GetProperties().Any(_ =>
                    Attribute.GetCustomAttribute(_, typeof(DocumentFlagAttribute)) != null));
            var result = typeWithDocuments
                .Select(_ => new DocumentTypeAndColumnDto
                {
                    Type = _,
                    ColumnSpecifiers = _.GetProperties()?.Where(_ =>
                        Attribute.GetCustomAttribute(_, typeof(DocumentFlagAttribute)) != null)?
                            .Select(_ => _.Name).ToList()
                }).ToList();

            return result ?? new List<DocumentTypeAndColumnDto>();
        }

        private async Task DeleteUnReferencedDocuments(EFDataContext dataContext, CancellationToken stoppingToken,
            List<Guid> unReferencedDocumentIds)
        {
            foreach (var unReferencedDocumentId in unReferencedDocumentIds)
            {
                try
                {
                    await dataContext.Database
                        .ExecuteSqlRawAsync(
                            $"DELETE FROM Documents WHERE Id = N'{unReferencedDocumentId}' And DATEDIFF(HOUR,CreationDate,N'{DateTime.UtcNow}') > 1",
                            stoppingToken);

                    _logger.LogInformation($"Document with id : {unReferencedDocumentId} successfully deleted");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Delete document with id :{unReferencedDocumentId} failed with error : {e.Message}");
                }
            }
        }

        private List<Guid> FindUnReferencedDocumentIds(List<Guid> allDocumentIds, List<Guid> referencedDocumentIds)
        {
            return allDocumentIds.Except(referencedDocumentIds).ToList();
        }

        private List<Guid> GetAllDocumentIds(EFDataContext dataContext)
        {
            return dataContext.Set<Document>().Select(_ => _.Id).ToList();
        }

        private List<Guid> FindReferencedDocumentIds(EFDataContext dataContext, List<DocumentTypeAndColumnDto> documentTypes)
        {
            List<Guid> referencedDocumentIds = new List<Guid>();
            foreach (var entity in documentTypes)
            {
                foreach (var columnName in entity.ColumnSpecifiers)
                {
                    referencedDocumentIds = referencedDocumentIds.Union(dataContext.Set(entity.Type)
                    .Where($"_ =>  _.{columnName} != null")
                    .Select($"_ => _.{columnName}").ToDynamicList<Guid>()).ToList();
                }
            }

            return referencedDocumentIds;
        }
    }

    class DocumentTypeAndColumnDto
    {
        public Type Type { get; set; }
        public List<string> ColumnSpecifiers { get; set; }
    }
}