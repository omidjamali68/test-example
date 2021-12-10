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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
