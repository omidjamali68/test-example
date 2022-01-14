using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Nationalities.Contracts
{
    public interface INationalityService : IService
    {
        Task<PageResult<GetAllNationalityDto>> GetAll(
            string searchText,
            Pagination pagination,
            Sort<GetAllNationalityDto> sortExpression);
    }
}