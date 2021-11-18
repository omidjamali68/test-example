using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.Ingredients.Ingredients.Contracts
{
    public interface IIngredientService : IService
    {
        Task<long> AddAsync(AddIngredientDto dto);
    }
}
