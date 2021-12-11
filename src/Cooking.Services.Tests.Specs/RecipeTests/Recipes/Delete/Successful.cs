using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using Cooking.TestTools.RecipeTestTools.RecipeCategories;
using Cooking.TestTools.RecipeTestTools.Recipes;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using Cooking.TestTools.StateTestTools;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.RecipeTests.Recipes.Delete
{
    [Scenario("حذف دستور پخت غذا")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IRecipeService _sut;
        private Ingredient _ingredientEgge;
        private Ingredient _ingredientOil;
        private StepOperation _stepOperation;
        private Recipe _recipe;
        private readonly Document _document;


        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = RecipeFactory.CreateService(_context);
            _document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
        }

        [Given("یک واحد مواد اولیه با عنوان: تعداد، در فهرست واحدهای مواد اولیه وجود دارد " +
            "و یک واحد مواد اولیه با عنوان: گرم، در فهرست واحدهای مواد اولیه وجود دارد")]
        [And("یک ماده اولیه با عنوان تخم مرغ با واحد تعداد در فهرست مواد اولیه وجود دارد" +
            "و یک ماده اولیه با عنوان روغن و واحد گرم در فهرست مواد اولیه وجود دارد")]
        private void Given()
        {
            var ingredientUnit_first = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientUnit_second = new IngredientUnitBuilder()
               .WithTitle("گرم")
               .Build(_context);
            _ingredientEgge = new IngredientBuilder(ingredientUnit_first.Id, _document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            _ingredientOil = new IngredientBuilder(ingredientUnit_second.Id, _document)
                .WithTitle("روغن")
                .Build(_context);
        }

        [And("یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود دارد")]
        private void And_StepOperation()
        {
            _stepOperation = new StepOperationBuilder(_document)
             .WithTitle("سرخ کردن")
             .Build(_context);
        }

        [And("فقط یک دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه در فهرست دستور پخت غذا ها وجود دارد")]
        private void And_Recipe()
        {
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            _recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(_ingredientEgge)
                .WithIngredient(_ingredientOil)
                .WithStep(_stepOperation)
                .Build(_context);
        }

        [When("دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه را از فهرست دستور پخت ها حذف میکنم")]
        private async Task When()
        {
           await _sut.DeleteAsync(_recipe.Id);
        }

        [Then("باید هیچ دستور پخت غذایی در فهرست دستور پخت ها وجود نداشته باشد")]
        private async Task Then()
        {
            var excepted = await _readDataContext.Recipes.ToListAsync();
            excepted.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_StepOperation(),
                _ => And_Recipe(),
                _ => When().Wait(),
                _ => Then().Wait()
                );
        }
    }
}
