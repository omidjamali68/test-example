using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Entities.Recipes;
using Cooking.Entities.States;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using Cooking.Specs;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using Cooking.TestTools.RecipeTestTools.RecipeCategories;
using Cooking.TestTools.RecipeTestTools.RecipeDocuments;
using Cooking.TestTools.RecipeTestTools.RecipeIngredients;
using Cooking.TestTools.RecipeTestTools.Recipes;
using Cooking.TestTools.RecipeTestTools.RecipeSteps;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using Cooking.TestTools.StateTestTools;
using FluentAssertions;
using Xunit;
using Document = Cooking.Entities.Documents.Document;

namespace Cooking.Services.Tests.Specs.RecipeTests.Recipes.Update
{
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IRecipeService _sut;
        private Ingredient _ingredientEgge;
        private Ingredient _ingredientOil;
        private StepOperation _stepOperation;
        private Recipe _recipe;
        private RecipeCategory _recipeCategory;
        private Nationality _nationality;
        private UpdateRecipeDto _dto;
        private readonly Document _document;
        public Successful(ConfigurationFixture configuration) 
        : base(configuration)
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
            _ingredientEgge = new IngredientBuilder(
                ingredientUnit_first.Id, _document)
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

        [And(" یک دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه در فهرست دستور پخت غذا ها وجود دارد")]
        private void And_Recipe_Exist()
        {
            _recipeCategory = new RecipeCategoryBuilder()
                .Build(_context);
            _nationality = new NationalityBuilder()
                .Build(_context);
            _recipe = new RecipeBuilder(_nationality.Id, _recipeCategory.Id)
                .WithIngredient(_ingredientEgge)
                .WithIngredient(_ingredientOil)
                .WithStep(_stepOperation)
                .Build(_context);
        }

        [When("دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه را به " +
        "دستور پخت غذا با نام غذا: نیمرو، زمان آماده سازی: 7 دقیقه ویرایش می کنم")]
        private async Task When()
        {
            var steps = new HashSet<RecipeStepDto>{
                RecipeStepFactory.GenerateDto(_stepOperation.Id)
            };
            var ingredients = new HashSet<RecipeIngredientDto>{
                RecipeIngredientFactory.GenerateDto(_ingredientOil.Id),
                RecipeIngredientFactory.GenerateDto(_ingredientEgge.Id)
            };
            var docs = new HashSet<RecipeDocumentDto>{
                RecipeDocumentFactory.GenerateDto(_document.Id)
            };
            _dto = RecipeFactory.GenerateUpdateDto(
                _nationality.Id,
                _recipeCategory.Id,
                ingredients,
                docs,
                steps);
            await _sut.Update(_dto, _recipe.Id);
        }

        [Then("باید فقط یک دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 7 در فهرست دستور پخت ها وجود داشته باشد")]
        private void Then()
        {
            var expected = _readDataContext.Recipes.First(_ => _.Id == _recipe.Id);
            expected.Duration.Should().Be(_dto.Duration);
            expected.FoodName.Should().Be(_dto.FoodName);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_StepOperation(),
                _ => And_Recipe_Exist(),
                _ => When().Wait(),
                _ => Then()
            );
        }
    }
}