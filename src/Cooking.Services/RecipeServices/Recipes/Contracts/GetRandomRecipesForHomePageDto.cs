using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetRandomRecipesForHomePageDto
    {
        public string FoodName { get; set; }
        public short? Duration { get; set; }
        public Guid MainDocumentId { get; set; }
        public string MainDocumentExtension { get; set; }
    }
}
