using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetRecipesByIngredientsDto
    {
        public string FoodName { get; set; }
        public short? Duration { get; set; }
        public Guid MainDocumentId { get; set; }
        public string MainDocumentExtension { get; set; }
        public IEnumerable<long> IngredientIds { get; set; }
    }
}
