using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public interface IRecipeRepository : IRepository
    {
        Task Add(Recipe recipe);
        void Remove(Recipe recipe);
        Task<Recipe> FindByIdAsync(long id);
        Task<GetRecipeDto> GetAsync(long id);
        Task<PageResult<GetAllRecipeDto>> GetAllAsync(
            string searchText,
            Pagination pagination,
            Sort<GetAllRecipeDto> sortExpression);
        Task<ICollection<GetAllRecipeDto>> GetAllByNationalityIdAsync(int nationalityId);
        Task<IList<GetRandomRecipesForHomePageDto>> GetRandomForHomePage();
    }
}
