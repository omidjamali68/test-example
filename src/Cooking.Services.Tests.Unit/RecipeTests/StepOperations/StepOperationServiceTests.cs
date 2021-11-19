using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Services.Tests.Unit.RecipeTests.StepOperations
{
    public class StepOperationServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly IStepOperationService _sut;
        public StepOperationServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Fact]
        private async Task Add_stepOperation_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            var dto = StepOperationFactory.GenerateAddDto("تفت دادن", document.Id);

            var addedId = await _sut.AddAsync(dto);

            var expected = _readContext.StepOperations.FirstOrDefault(_ => _.Id == addedId);
            expected.Title.Should().Be(dto.Title);
            expected.AvatarId.Should().Be(dto.AvatarId);
        }
    }
}
