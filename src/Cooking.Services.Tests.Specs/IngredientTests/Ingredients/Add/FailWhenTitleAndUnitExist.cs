using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.Services.IngredientServices.Ingredients.Exceptions;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.IngredientTests.Ingredients.Add
{
    [Scenario("جلوگیری از تعریف ماده اولیه غذا با عنوان و واحد تکراری")]
    public class FailWhenTitleAndUnitExist : EFDataContextDatabaseFixture
    {
        private readonly IIngredientService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private IngredientUnit _ingredientUnit;
        private AddIngredientDto _dto;
        private Func<Task> _expected;

        public FailWhenTitleAndUnitExist(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readContext = CreateDataContext();
            _sut = IngredientFactory.CreateService(_context);
        }

        [Given("یک واحد مواد اولیه با عنوان: تعداد، در فهرست واحدهای مواد اولیه وجود دارد")]
        private void Given()
        {
            _ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
        }

        [And("یک ماده اولیه با عنوان تخم مرغ با واحد تعداد در فهرست مواد اولیه وجود دارد")]
        private void And_Ingredient()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            new IngredientBuilder(_ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
        }

        [When("یک ماده اولیه با عنوان تخم مرغ با واحد تعداد تعریف می کنم")]
        private void When()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            _dto = IngredientFactory.GenerateAddIngredientDto(
                ingredientUnitId: _ingredientUnit.Id,
                title: "تخم مرغ",
                avatarId: document.Id);

            _expected = async () => await _sut.AddAsync(_dto);
        }

        [Then("باید خطای ‘ماده اولیه تکراری است’ رخ دهد")]
        [And("باید فقط یک ماده اولیه با عنوان تخم مرغ و واحد تعداد در فهرست مواد اولیه وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<IngredientTitleAndUnitExistException>();
            var dbExpected = _readContext.Ingredients
                .Where(_ => _.Title.Equals(_dto.Title) && _.IngredientUnitId == _dto.IngredientUnitId);
            dbExpected.Should().HaveCount(1);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_Ingredient(),
                _ => When(),
                _ => Then().Wait()
                );
        }
    }
}
