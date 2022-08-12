using System.Threading.Tasks;
using TestExample.Entities.Universities;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.Universities.Contracts
{
    public interface IUniversityRepository : IRepository
    {
        void Add(University university);
        Task<bool> IsNameExist(string name, int? id);
        Task<University> FindById(int id);
        Task<GetUniversityDto> GetById(int id);
        Task<PageResult<GetAllUniversitiesDto>> GetAll(
            Pagination? pagination,
            Sort<GetAllUniversitiesDto>? sort,
            string? search);
        void Delete(University university);
    }
}