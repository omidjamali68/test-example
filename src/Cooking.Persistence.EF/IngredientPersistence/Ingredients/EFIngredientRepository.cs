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
        private readonly EFDataContext _context;
        private readonly DbSet<Ingredient> _ingredients;

        public EFIngredientRepository(EFDataContext context)
        {
            _context = context;
            _ingredients = _context.Set<Ingredient>();
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            await _ingredients.AddAsync(ingredient);
        }
    }
}
