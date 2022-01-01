using Cooking.Infrastructure.Application;
using System.Threading.Tasks;

namespace Cooking.Services.IngredientServices.Ingredients.Contracts
{
    public interface IIngredientService : IService
    {
        Task<long> AddAsync(AddIngredientDto dto);
        Task UpdateAsync(long id, UpdateIngredientDto dto);
        Task DeleteAsync(long id);
        Task<GetIngredientDto> GetAsync(long id);
        Task<PageResult<GetAllIngredientDto>> GetAllAsync(
            string searchText,
            Pagination pagination,
            Sort<GetAllIngredientDto> sortExpression);
    }
}
