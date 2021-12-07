using Cooking.Persistence.EF;
using Cooking.Persistence.EF.RecipePersistence.Recipes;
using Cooking.Services.RecipeServices.Recipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.TestTools.RecipeTestTools.Recipes
{
    public static class RecipeFactory
    {
        public static RecipeAppService CreateService(EFDataContext context)
        {
            var repository = new EFRecipeRepository(context);
            var unitOfWork = new EFUnitOfWork(context);

            return new RecipeAppService(repository, unitOfWork);
        }
    }
}
