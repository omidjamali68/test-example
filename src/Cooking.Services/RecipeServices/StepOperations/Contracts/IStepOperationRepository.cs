using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public interface IStepOperationRepository : IRepository
    {
        Task AddAsync(StepOperation stepOperation);
        Task<StepOperation> FindById(long id);
        Task<bool> IsTitleExist(string title, long? id);
        void Remove(StepOperation stepOperation);
        Task<bool> ExistInRecipe(long stepOperationId);
        Task<PageResult<GetAllStepOperationDto>> GetAll(
            string searchText,
            Pagination? pagination,
            Sort<GetAllStepOperationDto>? sortExpression
            );
    }
}
