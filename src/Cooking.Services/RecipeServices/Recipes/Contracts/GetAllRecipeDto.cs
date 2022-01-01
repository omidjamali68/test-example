using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetAllRecipeDto
    {
        public long Id { get; set; }
        public string FoodName { get; set; }
        public short? Duration { get; set; }
        public string RecipeCategoryTitle { get; set; }
        public string NationalityName { get; set; }
    }
}
