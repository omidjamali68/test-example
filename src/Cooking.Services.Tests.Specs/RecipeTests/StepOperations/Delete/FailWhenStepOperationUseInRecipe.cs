using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Services.RecipeServices.StepOperations.Exceptions;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using Cooking.TestTools.RecipeTestTools.RecipeCategories;
using Cooking.TestTools.RecipeTestTools.Recipes;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using Cooking.TestTools.StateTestTools;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.RecipeTests.StepOperations.Delete
{
    [Scenario("جلوگیری از حذف مرحله پخت غذا استفاده شده در دستور پخت غذا")]
    public class FailWhenStepOperationUseInRecipe : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IStepOperationService _sut;
        private Document _avatarPic;
        private StepOperation _stepOperation;
        private Entities.Ingredients.Ingredient _ingredient;
        private Recipe _recipe;
        private Func<Task> _expected;
        public FailWhenStepOperationUseInRecipe(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان:سرخ کردن در فهرست مراحل پخت غذا وجود دارد")]
        [And("یک ماده اولیه با عنوان تخم مرغ با واحد تعداد در فهرست مواد اولیه وجود دارد")]
        private void Given()
        {
            _avatarPic = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            _stepOperation = new StepOperationBuilder(_avatarPic)
                .WithTitle("سرخ کردن")
                .Build(_context);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            _ingredient = new IngredientBuilder(ingredientUnit.Id, _avatarPic)
                .WithTitle("تخم مرغ")
                .Build(_context);
        }

        [And("یک دستور پخت دارای ماده اولیه تخم مرغ و مرحله پخت سرخ کردن در فهرست دستور پخت ها وجود دارد")]
        private void And_Recipe_Exist()
        {
            var recipeCategory = new RecipeCategoryBuilder()
                .Build(_context);
            var nationality = new NationalityBuilder()
                .Build(_context);
            _recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(_ingredient)
                .WithStep(_stepOperation)
                .Build(_context);
        }

        [When("مرحله پخت غذا با عنوان: سرخ کردن را از فهرست مراحل پخت غذا حذف میکنم")]
        private async Task When()
        {
            Func<Task> _expected = async () => await _sut.Delete(_stepOperation.Id);
        }

        [Then("باید خطای ‘مرحله پخت دارای قرارداد میباشد’ رخ دهد")]
        [And("یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<StepOperationUseInRecipeException>();
            var dbExpected = _readDataContext.StepOperations.Where(_ => _.Id == _stepOperation.Id);
            dbExpected.Should().HaveCount(1);
            dbExpected.First().Title.Should().Be(_stepOperation.Title);
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
