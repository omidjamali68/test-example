using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Web;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cooking.RestApi.Controllers.V1.Ingredients
{
    [ApiVersion("1.0")]
    [ApiController, Route("/api/v{version:apiVersion}/ingredients")]
    [Authorize]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _service;

        public IngredientsController(IIngredientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Add(AddIngredientDto dto)
        {
            await _service.AddAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(long id, UpdateIngredientDto dto)
        {
            await _service.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<GetIngredientDto> Get(long id)
        {
            return await _service.GetAsync(id);
        }

        [HttpGet]
        public async Task<PageResult<GetAllIngredientDto>> GetAll(
            [FromQuery] string searchText,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string sort
            )
        {
            var sortParser = new UriSortParser();
            var sortExpression = sort == null ? null : sortParser.Parse<GetAllIngredientDto>(sort);
            var pagination = limit.HasValue && offset.HasValue ? Pagination.Of(offset.Value + 1, limit.Value) : null;
            return await _service.GetAllAsync(searchText, pagination, sortExpression);
        }
    }
}
