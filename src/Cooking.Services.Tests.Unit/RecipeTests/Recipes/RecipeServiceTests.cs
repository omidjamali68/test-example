using Cooking.Entities.Documents;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Services.Tests.Unit.RecipeTests.Recipes
{
    public class RecipeServiceTests
    {
        private readonly IRecipeService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        public RecipeServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = RecipeFactory.CreateService(_context);
        }

        [Fact]
        private async Task Add_recipe_properly()
        {
            var doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var step = new StepOperationBuilder(doc)
                .WithTitle("سرخ کردن")
                .Build(_context);
            var firstIngredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
             var secondIngredientUnit = new IngredientUnitBuilder()
                .WithTitle("گرم")
                .Build(_context);
            var nationality = new NationalityBuilder()
                .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder()
                .Build(_context);
            var firstIngredient = new IngredientBuilder(firstIngredientUnit.Id, doc)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var secondIngredient = new IngredientBuilder(secondIngredientUnit.Id, doc)
                .WithTitle("روغن")
                .Build(_context);
            var steps = new HashSet<RecipeStepDto>
            {
                RecipeStepFactory.GenerateDto(step.Id)
            };
            var ingredients = new HashSet<RecipeIngredientDto>
            {
                RecipeIngredientFactory.GenerateDto(firstIngredient.Id, 2),
                RecipeIngredientFactory.GenerateDto(secondIngredient.Id, 1)
            };
            var documents = new HashSet<RecipeDocumentDto>
            {
                RecipeDocumentFactory.GenerateDto(doc.Id)
            };
            var dto = RecipeFactory.GenerateAddDto(
                "نیمرو",
                5,
                recipeCategory.Id,
                nationality.Id,
                ingredients,
                documents,
                steps
                );

            var addedRecipeId = await _sut.Add(dto);

            var expected = _readContext.Recipes
                .Include(_ => _.RecipeSteps)
                .Include(_ => _.RecipeDocuments)
                .Include(_ => _.RecipeIngredients)
                .FirstOrDefault(_ => _.Id == addedRecipeId);
            expected.Duration.Should().Be(dto.Duration);
            expected.FoodName.Should().Be(dto.FoodName);
            expected.NationalityId.Should().Be(dto.NationalityId);
            expected.RecipeCategoryId.Should().Be(dto.RecipeCategoryId);
            expected.RecipeSteps.Should()
                .Contain(_ => _.Description == dto.RecipeSteps.First().Description);
            expected.RecipeDocuments.Should()
                .Contain(_ => _.DocumentId == dto.RecipeDocuments.First().DocumentId);
            expected.RecipeIngredients.Should()
                .Contain(_ => _.IngredientId == dto.RecipeIngredients.First().IngredientId);
        }

        [Fact]
        private async Task Delete_remove_recipe_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit_first = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientUnit_second = new IngredientUnitBuilder()
               .WithTitle("گرم")
               .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit_first.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var ingredientOil = new IngredientBuilder(ingredientUnit_second.Id, document)
                .WithTitle("روغن")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(document)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithIngredient(ingredientOil)
                .WithStep(stepOperation)
                .Build(_context);

            await _sut.DeleteAsync(recipe.Id);

            var excepted = await _readContext.Recipes.ToListAsync();
            excepted.Should().BeNullOrEmpty();
        }

        [Fact]
        private async Task Get_get_recipe_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(document)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithStep(stepOperation)
                .WithDocument(document)
                .Build(_context);
            
            var expected = await _sut.GetAsync(recipe.Id);
                        
            expected.FoodName.Should().Be(recipe.FoodName);
            expected.Duration.Should().Be(recipe.Duration);
            expected.RecipeCategoryId.Should().Be(recipe.RecipeCategoryId);
            expected.NationalityId.Should().Be(recipe.NationalityId);
            expected.RecipeIngredients.Should().Contain(_ => 
            _.IngredientId == recipe.RecipeIngredients.First().IngredientId &&
            _.Quantity == recipe.RecipeIngredients.First().Quantity);
            expected.RecipeDocuments.Should().Contain(_ =>
            _.DocumentId == recipe.RecipeDocuments.First().DocumentId &&
            _.Extension == recipe.RecipeDocuments.First().Extension);
            expected.RecipeSteps.Should().Contain(_ =>
            _.Order == recipe.RecipeSteps.First().Order &&
            _.Description == recipe.RecipeSteps.First().Description &&
            _.StepOperationId == recipe.RecipeSteps.First().StepOperationId);
        }

        [Fact]
        private async Task Update_recipe_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit_first = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientUnit_second = new IngredientUnitBuilder()
               .WithTitle("گرم")
               .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit_first.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var ingredientOil = new IngredientBuilder(ingredientUnit_second.Id, document)
                .WithTitle("روغن")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(document)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithIngredient(ingredientOil)
                .WithStep(stepOperation)
                .Build(_context);
            var stepsDto = new HashSet<RecipeStepDto>{
                RecipeStepFactory.GenerateDto(stepOperation.Id)
            };
            var ingredientsDto = new HashSet<RecipeIngredientDto>{
                RecipeIngredientFactory.GenerateDto(ingredientOil.Id),
                RecipeIngredientFactory.GenerateDto(ingredientEgge.Id)
            };
            var docsDto = new HashSet<RecipeDocumentDto>{
                RecipeDocumentFactory.GenerateDto(document.Id)
            };
            var dto = RecipeFactory.GenerateUpdateDto(
                nationality.Id,
                recipeCategory.Id,
                ingredientsDto,
                docsDto,
                stepsDto,
                duration: 7);

            await _sut.Update(dto, recipe.Id);

            var expected = _readContext.Recipes.First(_ => _.Id == recipe.Id);
            expected.Duration.Should().Be(dto.Duration);
            expected.FoodName.Should().Be(dto.FoodName);
        }

        [Fact]
        private async Task Get_getAll_recipes_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(document)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithStep(stepOperation)
                .WithDocument(document)
                .Build(_context);

            var expected = await _sut.GetAllAsync(
                searchText: null,
                pagination: null,
                sortExpression: null);

            expected.Elements.Should().HaveCount(1);
            expected.Elements.Should().Contain(_ =>
            _.Id == recipe.Id &&
            _.FoodName == recipe.FoodName &&
            _.Duration == recipe.Duration &&
            _.RecipeCategoryTitle == recipe.RecipeCategory.Title &&
            _.NationalityName == recipe.Nationality.Name);
        }

        [Fact]
        private async Task Get_get_10_random_recipes_properly()
        {
            var otherDocument = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var mainDocument = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit.Id, otherDocument)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(otherDocument)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);

            for(int i =1; i <= 11; i++)
            {
                var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithStep(stepOperation)
                .WithDocument(otherDocument)
                .WithFoodName("نیمرو" + i.ToString())
                .WithMainDocument(mainDocument)
                .Build(_context);
            }

            var expected = await _sut.GetRandomForHomePage();

            expected.Should().HaveCount(10);
        }

        [Fact]
        private async Task Get_returns_recipes_by_nationalityId()
        {
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var ingredientEgge = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var stepOperation = new StepOperationBuilder(document)
            .WithTitle("سرخ کردن")
            .Build(_context);
            var recipeCategory = new RecipeCategoryBuilder().Build(_context);
            var nationality = new NationalityBuilder().Build(_context);
            var recipe = new RecipeBuilder(nationality.Id, recipeCategory.Id)
                .WithIngredient(ingredientEgge)
                .WithStep(stepOperation)
                .WithDocument(document)
                .Build(_context);

            var expected = await _sut.GetAllByNationalityIdAsync(nationality.Id);

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ =>
            _.Id == recipe.Id &&
            _.FoodName == recipe.FoodName &&
            _.Duration == recipe.Duration &&
            _.RecipeCategoryTitle == recipe.RecipeCategory.Title &&
            _.NationalityName == recipe.Nationality.Name &&
            _.MainDocumentId == recipe.MainDocumentId &&
            _.MainDocumentExtension == recipe.MainDocumentExtension);
        }
    }
}
