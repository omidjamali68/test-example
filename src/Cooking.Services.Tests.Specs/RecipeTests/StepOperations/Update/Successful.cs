using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
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

namespace Cooking.Specs.RecipeTests.StepOperations.Update
{
    [Scenario("ویرایش مرحله پخت غذا")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private Document _avatarPic;
        private StepOperation _stepOperation;
        private UpdateStepOperationDto _dto;

        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان: تفت دادن در فهرست مراحل پخت غذا وجود دارد")]
        private void Given()
        {
            _avatarPic = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            _stepOperation = new StepOperationBuilder(_avatarPic)
                .WithTitle("تفت دادن")
                .Build(_context);
        }

        [When("مرحله پخت غذا با عنوان: تفت دادن را به مرحله پخت غذا با عنوان: سرخ کردن ویرایش میکنم")]
        private async Task When()
        {
            _dto = StepOperationFactory.GenerateUpdateDto(_avatarPic, "سرخ کردن");
            await _sut.UpdateAsync(_dto, _stepOperation.Id);
        }

        [Then("باید فقط یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود داشته باشد")]
        private void Then()
        {
            var expected = _readDataContext.StepOperations.First(_ => _.Id == _stepOperation.Id);
            expected.Title.Should().Be(_dto.Title);
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
