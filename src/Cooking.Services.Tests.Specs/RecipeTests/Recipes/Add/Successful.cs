using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.Recipes.Recipes.Add
{
    [Scenario("تعریف دستور پخت غذا به همراه فایل تصاویر")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IRecipeService _sut;
        private IngredientUnit _firstIngredientUnit;
        private IngredientUnit _secondIngredientUnit;
        private Document _doc;
        private Entities.Recipes.StepOperation _step;
        private Ingredient _firstIngredient;
        private Ingredient _secondIngredient;
        private AddRecipeDto _dto;
        private long _addedRecipeId;

        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = RecipeFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود دارد")]
        [And("یک واحد مواد اولیه با عنوان: تعداد، در فهرست واحدهای مواد اولیه وجود دارد")]
        private void Given()
        {
            _doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            _step = new StepOperationBuilder(_doc)
                .WithTitle("سرخ کردن")
                .Build(_context);
            _firstIngredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
        }

        [And("یک واحد مواد اولیه با عنوان: گرم، در فهرست واحدهای مواد اولیه وجود دارد")]
        private void And_IngredientUnit_Exist()
        {
            _secondIngredientUnit = new IngredientUnitBuilder()
                .WithTitle("گرم")
                .Build(_context);
        }

        [And("یک ماده اولیه با عنوان تخم مرغ و واحد تعداد در فهرست مواد اولیه وجود دارد")]
        private void And_First_Ingredient_Exist()
        {
            _firstIngredient = new IngredientBuilder(_firstIngredientUnit.Id, _doc)
                .WithTitle("تخم مرغ")
                .Build(_context);
        }

        [And("یک ماده اولیه با عنوان روغن و واحد گرم در فهرست مواد اولیه وجود دارد")]
        private void And_Second_Ingredient_Exist()
        {
            _secondIngredient = new IngredientBuilder(_secondIngredientUnit.Id, _doc)
                .WithTitle("روغن")
                .Build(_context);
        }

        [When("یک دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه، برای " +
            "تعداد: 2نفر و گروه دستور پخت: حاضری و کشور: ایران و مرحله" +
        " پخت با عنوان: سرخ کردن به همراه فایل های مربوطه در فهرست دستور پخت ها تعریف میکنم")]
        private async Task When()
        {
            var nationality = new NationalityBuilder()
                .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder()
                .Build(_context);
            var steps = new HashSet<RecipeStepDto>
            {
                RecipeStepFactory.GenerateDto(_step.Id)
            };
            var ingredients = new HashSet<RecipeIngredientDto>
            {
                RecipeIngredientFactory.GenerateDto(_firstIngredient.Id, 2),
                RecipeIngredientFactory.GenerateDto(_secondIngredient.Id, 1)
            };
            var documents = new HashSet<RecipeDocumentDto>
            {
                RecipeDocumentFactory.GenerateDto(_doc.Id)
            };
            var mainDocument = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            _dto = new RecipeAddDtoBuilder()
                .WithCategoryId(recipeCategory.Id)
                .WithDuration(5)
                .WithFooodName("نیمرو")
                .WithForHowManyPeople(2)
                .WithNationalityId(nationality.Id)
                .WithSteps(steps)
                .WithIngredients(ingredients)
                .WithDocuments(documents)
                .WithMainDocument(mainDocument)
                .Build();
                
            _addedRecipeId = await _sut.Add(_dto);
        }

        [Then("باید یک دستور پخت با نام غذا: نیمرو،" +
        " زمان آماده سازی: 5 دقیقه، برای تعداد: 2نفر و گروه دستور پخت: حاضری و کشور: ایران و مرحله پخت" +
        " با عنوان: سرخ کردن به همراه فایل های مربوطه در فهرست دستور پخت ها وجود داشته باشد")]
        private void Then()
        {
            var expected = _readDataContext.Recipes
                .Include(_ => _.RecipeSteps)
                .Include(_ => _.RecipeDocuments)
                .Include(_ => _.RecipeIngredients)
                .FirstOrDefault(_ => _.Id == _addedRecipeId);
            expected.Duration.Should().Be(_dto.Duration);
            expected.FoodName.Should().Be(_dto.FoodName);
            expected.MainDocumentId.Should().Be(_dto.MainDocumentId);
            expected.NationalityId.Should().Be(_dto.NationalityId);
            expected.ForHowManyPeople.Should().Be(_dto.ForHowManyPeople);
            expected.RecipeCategoryId.Should().Be(_dto.RecipeCategoryId);
            expected.RecipeSteps.Should()
                .Contain(_ => _.Description == _dto.RecipeSteps.First().Description);
            expected.RecipeDocuments.Should()
                .Contain(_ => _.DocumentId == _dto.RecipeDocuments.First().DocumentId);
            expected.RecipeIngredients.Should()
                .Contain(_ => _.IngredientId == _dto.RecipeIngredients.First().IngredientId);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_IngredientUnit_Exist(),
                _ => And_First_Ingredient_Exist(),
                _ => And_Second_Ingredient_Exist(),
                _ => When().Wait(),
                _ => Then()
                );
        }
    }
}
