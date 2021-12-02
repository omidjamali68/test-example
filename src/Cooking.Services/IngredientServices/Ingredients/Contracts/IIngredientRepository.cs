using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.IngredientServices.Ingredients.Contracts
{
    public interface IIngredientRepository : IRepository
    {
        Task AddAsync(Ingredient ingredient);
        Task<Ingredient> FindByIdAsync(long id);
        void Remove(Ingredient ingredient);
        Task<bool> IsTitleAndUnitExistAsync(string title, int ingredientUnitId, long? id);
        Task<GetIngredientDto> GetAsync(long id);
    }
}
