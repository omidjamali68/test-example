using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.TestTools.RecipeTestTools.RecipeIngredients;
using Cooking.TestTools.RecipeTestTools.RecipeSteps;

namespace Cooking.TestTools.RecipeTestTools.Recipes
{
    public class RecipeBuilder
    {
        private Recipe _recipe = new Recipe { 
            FoodName = "dummy_foodName",
        };

        public RecipeBuilder(long ingredientId, long stepOperationId)
        {
            _recipe.RecipeIngredients.Add(RecipeIngredientFactory.CreateInstance(ingredientId));
            _recipe.RecipeSteps.Add(RecipeStepFactory.CreateInstance(stepOperationId));
        }

        public RecipeBuilder WithNationality(int id)
        {
            _recipe.NationalityId = id;
            return this;
        }

        public RecipeBuilder WithCategory(int id)
        {
            _recipe.RecipeCategoryId = id;
            return this;
        }

        public Recipe Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Recipes.Add(_recipe));
            return _recipe;
        }        
    }
}
