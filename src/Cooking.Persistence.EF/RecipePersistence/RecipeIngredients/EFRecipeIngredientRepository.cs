using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.RecipePersistence.RecipeIngredients
{
    public class EFRecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly DbSet<RecipeIngredient> _recipeIngredients;
        public EFRecipeIngredientRepository(EFDataContext context)
        {
            _recipeIngredients = context.RecipeIngredients;
        }

        public async Task<RecipeIngredient> FindByIngredientIdAsync(long ingredientId)
        {
            return await _recipeIngredients
                .FirstOrDefaultAsync(_ => _.IngredientId == ingredientId);
        }
    }
}
