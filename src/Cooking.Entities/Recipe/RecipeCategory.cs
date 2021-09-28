using System.Collections.Generic;

namespace Cooking.Entities.Recipe
{
    public class RecipeCategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public string Title {  get; set; }

        public HashSet<Recipe> Recipes { get; set; }
    }
}
