using Cooking.Entities.Documents;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.TestTools.RecipeTestTools.Recipes
{
    public class RecipeAddDtoBuilder
    {
        private AddRecipeDto dto = new AddRecipeDto
        {

        };

        public RecipeAddDtoBuilder WithFooodName(string foodName)
        {
            dto.FoodName = foodName;
            return this;
        }

        public RecipeAddDtoBuilder WithDuration(short duration)
        {
            dto.Duration = duration;
            return this;
        }

        public RecipeAddDtoBuilder WithCategoryId(int categoryId)
        {
            dto.RecipeCategoryId = categoryId;
            return this;
        }

        public RecipeAddDtoBuilder WithNationalityId(int nationalityId)
        {
            dto.NationalityId = nationalityId;
            return this;
        }
        
        public RecipeAddDtoBuilder WithForHowManyPeople(byte forHowManyPeople)
        {
            dto.ForHowManyPeople = forHowManyPeople;
            return this;
        }

        public RecipeAddDtoBuilder WithIngredients(HashSet<RecipeIngredientDto> ingredients)
        {
            dto.RecipeIngredients = ingredients;
            return this;
        }

        public RecipeAddDtoBuilder WithDocuments(HashSet<RecipeDocumentDto> recipeDocuments)
        {
            dto.RecipeDocuments = recipeDocuments;
            return this;
        }

        public RecipeAddDtoBuilder WithSteps(HashSet<RecipeStepDto> recipeSteps)
        {
            dto.RecipeSteps = recipeSteps;
            return this;
        }

        public RecipeAddDtoBuilder WithMainDocument(Document mainDocument)
        {
            dto.MainDocumentId = mainDocument.Id;
            dto.MainDocumentExtension = mainDocument.Extension;
            return this;
        }

        public AddRecipeDto Build()
        {
            return dto;
        }

    }
}
