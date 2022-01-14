using Cooking.Persistence.EF;
using Cooking.Persistence.EF.RecipePersistence.RecipeCategories;
using Cooking.Services.RecipeServices.RecipeCtegories;

namespace Cooking.TestTools.RecipeTestTools.RecipeCategories
{
    public class RecipeCategoryFactory
    {
        public static RecipeCategoryAppService CreateService(EFDataContext context)
        {
            var repository = new EFRecipeCategoryRepository(context);
            return new RecipeCategoryAppService(repository);
        }
    }
}