using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using System;
using System.Collections.Generic;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetRecipeDto
    {
        public string FoodName { get; set; }
        public short? Duration { get; set; }
        public byte ForHowManyPeople { get; set; }
        public int RecipeCategoryId { get; set; }
        public int NationalityId { get; set; }
        public Guid MainDocumentId { get; set; }
        public string MainDocumentExtension { get; set; }
        public HashSet<RecipeIngredientDto> RecipeIngredients { get; set; }
        public HashSet<RecipeDocumentDto> RecipeDocuments { get; set; }
        public HashSet<RecipeStepDto> RecipeSteps { get; set; }
    }
}
