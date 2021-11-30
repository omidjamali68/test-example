using Cooking.Infrastructure.Application;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public interface IStepOperationService : IService
    {
        Task<long> AddAsync(AddStepOperationDto dto);
        Task UpdateAsync(UpdateStepOperationDto dto, long id);
        Task Delete(long id);
        Task<GetStepOperationDto> GetStepOperation(long id);
        Task<PageResult<GetAllStepOperationDto>> GetAllStepOperation(
            string searchText,
            Pagination? pagination,
            Sort<GetAllStepOperationDto>? sortExpression
            );
    }
}
