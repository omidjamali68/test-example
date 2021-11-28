using Cooking.Entities.Recipes;

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
    }
}
