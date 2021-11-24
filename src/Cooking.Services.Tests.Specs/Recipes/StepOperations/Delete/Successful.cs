using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.Recipes.StepOperations.Delete
{
    [Scenario("حذف مرحله پخت غذا")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private Document _avatarPic;
        private StepOperation _stepOperation;
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

        [When("مرحله پخت غذا با عنوان: تفت دادن را از فهرست مراحل پخت غذا حذف میکنم")]
        private async Task When()
        {
            await _sut.Delete(_stepOperation.Id);
        }

        [Then("نباید هیچ مرحله ای در فهرست مراحل پخت غذا وجود داشته باشد")]
        private void Then()
        {
            var expected = _readDataContext.StepOperations.Where(_ => _.Id == _stepOperation.Id);
            expected.Should().BeNullOrEmpty();
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
