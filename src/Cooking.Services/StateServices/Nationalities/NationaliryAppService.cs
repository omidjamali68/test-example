using System.Threading.Tasks;
using Cooking.Infrastructure.Application;
using Cooking.Services.StateServices.Nationalities.Contracts;

namespace Cooking.Services.StateServices.Nationalities
{
    public class NationaliryAppService : INationalityService
    {
        private readonly INationalityRepository _repository;

        public NationaliryAppService(INationalityRepository repository)
        {
            _repository = repository;
        }

        public async Task<PageResult<GetAllNationalityDto>> GetAll(
            string searchText,
             Pagination pagination,
              Sort<GetAllNationalityDto> sortExpression)
        {
            return await _repository.GetAll(searchText, pagination, sortExpression);
        }
    }
}