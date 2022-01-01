using System.Threading.Tasks;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Web;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cooking.RestApi.Controllers.V1.Recipes
{
    [ApiVersion("1.0")]
    [ApiController, Route("/api/v{version:apiVersion}/recipes")]
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _service;

        public RecipeController(IRecipeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<long> Add(AddRecipeDto dto)
        {
            return await _service.Add(dto);
        }

        [HttpGet("{id}")]
        public async Task<GetRecipeDto> Get(long id)
        {
            return await _service.GetAsync(id);
        }

        [HttpPut("{id}")]
        public async Task Update(long id, UpdateRecipeDto dto)
        {
            await _service.Update(dto, id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<PageResult<GetAllRecipeDto>> GetAll(
           [FromQuery] string searchText,
           [FromQuery] int? limit,
           [FromQuery] int? offset,
           [FromQuery] string sort
           )
        {
            var sortParser = new UriSortParser();
            var sortExpression = sort == null 
                ? null 
                : sortParser.Parse<GetAllRecipeDto>(sort);
            var pagination = limit.HasValue && offset.HasValue 
                ? Pagination.Of(offset.Value + 1, limit.Value) 
                : null;
            return await _service.GetAllAsync(
                searchText,
                pagination,
                sortExpression);
        }
    }
}