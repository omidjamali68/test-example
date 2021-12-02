using Cooking.Entities.Ingredients;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<GetIngredientDto> GetAsync(long id)
        {
            return await _ingredients.Where(_ => _.Id == id)
                .Select(_ => new GetIngredientDto
                {
                    IngredientUnitId = _.IngredientUnitId,
                    Title = _.Title,
                    AvatarId = _.AvatarId,
                    Extension = _.Extension
                }).SingleOrDefaultAsync();
        }

        public async Task<bool> IsTitleAndUnitExistAsync(string title, int ingredientUnitId, long? id)
        {
            return await _ingredients
                .AnyAsync(_ => _.Title.Replace(" ", "").Equals(title.Replace(" ", "")) 
                && _.IngredientUnitId == ingredientUnitId
                && _.Id != id);
        }

        public void Remove(Ingredient ingredient)
        {
            _ingredients.Remove(ingredient);
        }
    }
}
