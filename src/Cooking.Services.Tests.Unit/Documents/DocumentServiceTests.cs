using System;
using Cooking.Entities.Documents;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.Documents;
using Cooking.Services.Documents;
using Cooking.Services.Documents.Contracts;
using Cooking.TestTools.DocumentTestTools;
using FluentAssertions;
using Xunit;

namespace Cooking.Services.Tests.Unit.Documents
{
    public class DocumentServiceTests
    {
        public DocumentServiceTests()
        {
            _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
            var documentRepository = new EFDocumentRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);

            _sut = new DocumentAppService(
                documentRepository,
                new UtcDateTimeService(),
                unitOfWork);
        }

        private readonly EFDataContext _context;
        private readonly DocumentService _sut;

    }
}