using Cooking.Entities.Documents;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.RecipePersistence.Recipes;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using System;
using System.Collections.Generic;

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

        public static AddRecipeDto GenerateAddDto(string foodName,
             short duration,
             int categoryId,
             int nationalityId, 
             HashSet<RecipeIngredientDto> ingredients,
             HashSet<RecipeDocumentDto> documents,
             HashSet<RecipeStepDto> steps
             )
        {
            return new AddRecipeDto{
                Duration = duration,
                FoodName = foodName,
                NationalityId = nationalityId,
                RecipeCategoryId = categoryId,
                RecipeDocuments = documents,
                RecipeIngredients = ingredients,
                RecipeSteps = steps
            };
        }
    }
}
