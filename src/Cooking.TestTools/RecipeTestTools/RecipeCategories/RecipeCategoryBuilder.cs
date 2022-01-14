using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.RecipeTestTools.RecipeCategories
{
    public class RecipeCategoryBuilder
    {
        private RecipeCategory _recipeCategory = new RecipeCategory { 
            Title = "dummy_title"
        };

        public RecipeCategoryBuilder WithTitle(string title)
        {
            _recipeCategory.Title = title ;
            return this;
        }
        public RecipeCategory Build(EFDataContext context)
        {
            context.Manipulate(_ => _.RecipeCategories.Add(_recipeCategory));
            return _recipeCategory;
        }
    }
}
