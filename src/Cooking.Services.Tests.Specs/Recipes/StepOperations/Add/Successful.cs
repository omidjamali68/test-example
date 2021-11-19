using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.Recipes.StepOperations.Add
{
    [Scenario("تعریف مرحله پخت غذا به همراه تصویر آیکون")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private AddStepOperationDto _dto;
        private long _addedId;

        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Given("هیچ مرحله پخت غذایی در فهرست مراحل پخت غذا وجود ندارد")]
        private void Given()
        {

        }

        [When("یک مرحله پخت غذا با عنوان: تفت دادن و تصویرمربوطه را تعریف میکنم")]
        private async Task When()
        {
            var document = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            _dto = StepOperationFactory.GenerateAddDto("تفت دادن" , document.Id);
            _addedId = await _sut.AddAsync(_dto);
        }

        [Then("باید فقط یک مرحله پخت غذا با عنوان: تفت دادن و تصویرمربوطه در فهرست مراحل پخت غذا وجود داشته باشد")]
        private void Then()
        {
            var expected = _readDataContext.StepOperations.FirstOrDefault(_ => _.Id == _addedId);
            expected.Title.Should().Be(_dto.Title);
            expected.AvatarId.Should().Be(_dto.AvatarId);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => When().Wait(),
                _ => Then()
                );
        }
    }
}
