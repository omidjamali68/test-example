using System;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetRecipeIngredientDto
    {
        public double Quantity { get; set; }
        public long IngredientId { get; set; }
        public Guid AvatarId { get; set; }
        public string Title { get; set; }
        public string UnitTitle { get; set; }
    }
}