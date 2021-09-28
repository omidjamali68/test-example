using Cooking.Entities.Ingredients;

namespace Cooking.Entities.Recipe
{
    public class RecipeIngredient
    {
        public double Quantity { get; set; }

        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public long IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
