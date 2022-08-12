using System.Threading.Tasks;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.Universities.Contracts
{
    public interface IUniversityService : IService
    {
        Task<int> Add(AddUniversityDto dto);
        Task Update(int id, UpdateUniversityDto dto);
        Task<GetUniversityDto> GetById(int id);
        Task<PageResult<GetAllUniversitiesDto>> GetAll(
            Pagination? pagination = null,
            Sort<GetAllUniversitiesDto>? sort = null,
            string? search = null);
        Task Delete(int id);
    }
}