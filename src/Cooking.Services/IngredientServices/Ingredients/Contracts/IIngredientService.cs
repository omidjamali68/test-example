using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.IngredientServices.Ingredients.Contracts
{
    public interface IIngredientService : IService
    {
        Task<long> AddAsync(AddIngredientDto dto);
        Task UpdateAsync(long id, UpdateIngredientDto dto);
        Task DeleteAsync(long id);
    }
}
