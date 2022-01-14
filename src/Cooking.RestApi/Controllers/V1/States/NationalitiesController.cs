using System.Threading.Tasks;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Web;
using Cooking.Services.StateServices.Nationalities.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Cooking.RestApi.Controllers.V1.States
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/nationalities")]
    public class NationalitiesController : Controller
    {
        private readonly INationalityService _service;

        public NationalitiesController(INationalityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<PageResult<GetAllNationalityDto>> GetAll(
            [FromQuery] string searchText,
           [FromQuery] int? limit,
           [FromQuery] int? offset,
           [FromQuery] string sort)
        {
            var sortParser = new UriSortParser();
            var sortExpression = sort == null 
                ? null 
                : sortParser.Parse<GetAllNationalityDto>(sort);
            var pagination = limit.HasValue && offset.HasValue 
                ? Pagination.Of(offset.Value + 1, limit.Value) 
                : null;
            return await _service.GetAll(searchText, pagination, sortExpression);
        }
    }
}