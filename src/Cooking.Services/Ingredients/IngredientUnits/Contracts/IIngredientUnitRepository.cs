using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.Ingredients.IngredientUnits.Contracts
{
    public interface IIngredientUnitRepository : IRepository
    {
        Task<IngredientUnit> FindByIdAsync(int id);
    }
}
