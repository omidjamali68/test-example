using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Services.RecipeServices.StepOperations.Exceptions;
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

namespace Cooking.Specs.RecipeTests.StepOperations.Add
{
    [Scenario("جلوگیری از تعریف مرحله پخت غذا با عنوان تکراری")]
    public class FailAddStepOperationWhenTitleExist : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private Document _document;
        private StepOperation _stepOperation;
        private AddStepOperationDto _dto;
        private Func<Task> _expected;

        public FailAddStepOperationWhenTitleExist(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان: تفت دادن در فهرست مراحل پخت غذا وجود دارد")]
        private void Given()
        {
            _document = DocumentFactory.CreateDocument(_context,
                Entities.Documents.DocumentStatus.Reserve);
            _stepOperation = new StepOperationBuilder(_document)
                .WithTitle("تفت دادن")
                .Build(_context);
        }

        [When("یک مرحله پخت غذا با عنوان: تفت دادن و تصویرمربوطه را تعریف میکنم")]
        private async Task When()
        {
            _dto = StepOperationFactory.GenerateAddDto("تفت دادن", _document.Id);
            _expected = async ()=> await _sut.AddAsync(_dto);
        }

        [Then("باید خطای ‘مرحله پخت غذا با عنوان تکراری’ رخ دهد")]
        [And("باید فقط یک مرحله پخت غذا با عنوان: تفت دادن در فهرست مراحل پخت غذا وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<StepOperationTitleExistException>();
            var dbExpected = _readDataContext.StepOperations.Where(_ => _.Title.Equals(_dto.Title));
            dbExpected.Should().HaveCount(1);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => When().Wait(),
                _ => Then().Wait()
                );
        }
    }
}
