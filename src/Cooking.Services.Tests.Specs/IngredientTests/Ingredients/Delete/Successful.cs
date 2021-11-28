using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.IngredientTests.Ingredients.Delete
{
    [Scenario("حذف ماده اولیه غذا")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly IIngredientService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private IngredientUnit _ingredientUnit;
        private Ingredient _ingredient;

        public Successful(ConfigurationFixture configuration) : base(configuration)
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
            _ingredient = new IngredientBuilder(_ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
        }

        [When("ماده اولیه با عنوان تخم مرغ با واحد تعداد را حذف می کنم")]
        private async Task When()
        {
            await _sut.DeleteAsync(_ingredient.Id);
        }

        [Then("نباید هیچ ماده اولیه ای در فهرست مواد اولیه وجود داشته باشد")]
        private async Task Then()
        {
            var expected = await _readContext.Ingredients
                .Where(_ => _.Id == _ingredient.Id)
                .ToListAsync();
            expected.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_Ingredient(),
                _ => When().Wait(),
                _ => Then().Wait()
                );
        }
    }
}
