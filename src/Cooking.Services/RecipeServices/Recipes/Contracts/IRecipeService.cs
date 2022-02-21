using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public interface IRecipeService : IService
    {
        Task<long> Add(AddRecipeDto dto);
        Task DeleteAsync(long id);
        Task Update(UpdateRecipeDto dto, long id);
        Task<GetRecipeDto> GetAsync(long id);
        Task<PageResult<GetAllRecipeDto>> GetAllAsync(
            string searchText,
            Pagination pagination,
            Sort<GetAllRecipeDto> sortExpression);
        Task<IList<GetRandomRecipesForHomePageDto>> GetRandomForHomePage();
        Task<ICollection<GetAllRecipeDto>> GetAllByNationalityIdAsync(int nationalityId);
        Task<ICollection<GetRecipesByIngredientsDto>> GetRecipesByIngredientsAsync(
            ICollection<long> ingredientIds);
    }
}
