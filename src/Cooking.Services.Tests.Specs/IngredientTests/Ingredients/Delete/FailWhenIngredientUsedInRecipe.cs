using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.Services.IngredientServices.Ingredients.Exceptions;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using Cooking.TestTools.RecipeTestTools.RecipeCategories;
using Cooking.TestTools.RecipeTestTools.RecipeIngredients;
using Cooking.TestTools.RecipeTestTools.Recipes;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using Cooking.TestTools.StateTestTools;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.IngredientTests.Ingredients.Delete
{
    [Scenario("جلوگیری از حذف ماده اولیه غذا استفاده شده در دستور پخت")]
    public class FailWhenIngredientUsedInRecipe : EFDataContextDatabaseFixture
    {
        private readonly IIngredientService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private IngredientUnit _ingredientUnit;
        private Ingredient _ingredient;
        private Func<Task> _expected;

        public FailWhenIngredientUsedInRecipe(ConfigurationFixture configuration) : base(configuration)
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

        [And("یک دستور پخت دارای ماده اولیه تخم مرغ و مرحله پخت سرخ کردن در فهرست دستور پخت ها وجود دارد")]
        private void And_Recipe()
        {
            var recipeCategory = new RecipeCategoryBuilder()
                .Build(_context);
            var nationality = new NationalityBuilder()
                .Build(_context);
            var avatar = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var stepOperation = new StepOperationBuilder(avatar)
                .WithTitle("سرخ کردن")
                .Build(_context);
            new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(_ingredient)
                .WithStep(stepOperation)
                .Build(_context);
        }

        [When("ماده اولیه با عنوان تخم مرغ با واحد تعداد را حذف می کنم")]
        private void When()
        {
            _expected = async () => await _sut.DeleteAsync(_ingredient.Id);
        }

        [Then("باید خطای ‘ماده اولیه دارای دستور پخت’ رخ دهد")]
        [And("باید یک ماده اولیه با عنوان تخم مرغ با واحد تعداد در فهرست مواد اولیه وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<IngredientUsedInRecipeException>();
            var dbExpected = _readContext.Ingredients
                .Where(_ => _.Title.Equals(_ingredient.Title) &&
                _.IngredientUnitId == _ingredient.IngredientUnitId);
            dbExpected.Should().HaveCount(1);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_Ingredient(),
                _ => And_Recipe(),
                _ => When(),
                _ => Then().Wait()
                );
        }
    }
}
