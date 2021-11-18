using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.Ingredients.Ingredients.Contracts
{
    public interface IIngredientRepository : IRepository
    {
        Task AddAsync(Ingredient ingredient);
    }
}
