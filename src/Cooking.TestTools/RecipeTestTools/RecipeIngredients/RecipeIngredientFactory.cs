using System;
using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;

namespace Cooking.TestTools.RecipeTestTools.RecipeIngredients
{
    public static class RecipeIngredientFactory
    {
        public static RecipeIngredient CreateInstance(long ingredientId)
        {
            return new RecipeIngredient
            {
                IngredientId = ingredientId
            };
        }

        public static RecipeIngredientDto GenerateDto(long ingredientId, double quantity = 2)
        {
            return new RecipeIngredientDto
            {
                IngredientId = ingredientId,
                Quantity = quantity
            };
        }
    }
}
