using Cooking.Entities.Ingredients;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.IngredientPersistence.Ingredients
{
    public class EFIngredientRepository : IIngredientRepository
    {
        private readonly DbSet<Ingredient> _ingredients;

        public EFIngredientRepository(EFDataContext context)
        {
            _ingredients = context.Ingredients;
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            await _ingredients.AddAsync(ingredient);
        }

        public async Task<Ingredient> FindByIdAsync(long id)
        {
            return await _ingredients.FindAsync(id);
        }

        public void Remove(Ingredient ingredient)
        {
            _ingredients.Remove(ingredient);
        }
    }
}
