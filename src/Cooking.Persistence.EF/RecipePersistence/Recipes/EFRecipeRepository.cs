using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.RecipePersistence.Recipes
{
    public class EFRecipeRepository : IRecipeRepository
    {
        private readonly DbSet<Recipe> _recipes;
        public EFRecipeRepository(EFDataContext context)
        {
            _recipes = context.Recipes;
        }

        public async Task Add(Recipe recipe)
        {
            await _recipes.AddAsync(recipe);
        }

        public async Task<Recipe> FindByIdAsync(long id)
        {
            return await _recipes.FindAsync(id);
        }

        public void Remove(Recipe recipe)
        {
            _recipes.Remove(recipe);
        }
    }
}
