using System.Collections.Generic;
using Cooking.Entities.States;

namespace Cooking.Entities.Recipe
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
            RecipeDocuments = new HashSet<RecipeDocument>();
            RecipeSteps = new HashSet<RecipeStep>();
        }

        public long Id { get; set; }
        public string FoodName { get; set; }
        public short Duration { get; set; }

        public int RecipeCategoryId { get; set; }
        public RecipeCategory RecipeCategory { get; set; }
        public int NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public HashSet<RecipeIngredient> RecipeIngredients { get; set; }
        public HashSet<RecipeDocument> RecipeDocuments { get; set; }
        public HashSet<RecipeStep> RecipeSteps { get; set; }
    }
}
