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
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.Recipes.StepOperations.Update
{
    [Scenario("جلوگیری از ویرایش مرحله پخت غذا با عنوان تکراری")]
    public class FailWhenTitleExist : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private Document _avatarPic;
        private StepOperation _firstStepOperation;
        private StepOperation _secondStepOperation;
        private UpdateStepOperationDto _dto;
        private Func<Task> _expected;

        public FailWhenTitleExist(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان: تفت دادن در فهرست مراحل پخت غذا وجود دارد")]
        [And("یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود دارد")]
        private void Given()
        {
            _avatarPic = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            _firstStepOperation = new StepOperationBuilder(_avatarPic)
                .WithTitle("تفت دادن")
                .Build(_context);
            _secondStepOperation = new StepOperationBuilder(_avatarPic)
                .WithTitle("سرخ کردن")
                .Build(_context);
        }

        [When("مرحله پخت غذا با عنوان: تفت دادن را به مرحله پخت غذا با عنوان: سرخ کردن ویرایش میکنم")]
        private async Task When()
        {
            _dto = StepOperationFactory.GenerateUpdateDto(_avatarPic, "سرخ کردن");
            _expected = async ()=> await _sut.UpdateAsync(_dto, _firstStepOperation.Id);
        }

        [Then("باید خطای ‘مرحله پخت غذا با عنوان تکراری ‘ رخ دهد")]
        [And("باید فقط یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<StepOperationTitleExistException>();
            var dbExpected = _readDataContext.StepOperations.First(_ => _.Title == _dto.Title);
            dbExpected.Should().BeEquivalentTo(_secondStepOperation);
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
