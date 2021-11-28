using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.RecipeIngredients.Contracts
{
    public interface IRecipeIngredientRepository : IRepository
    {
        Task<RecipeIngredient> FindByIngredientIdAsync(long ingredientId);
    }
}
