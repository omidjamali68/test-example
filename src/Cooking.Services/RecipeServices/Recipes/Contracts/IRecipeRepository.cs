using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public interface IRecipeRepository : IRepository
    {
        Task Add(Recipe recipe);
    }
}
