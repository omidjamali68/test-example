using System.Threading.Tasks;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Web;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cooking.RestApi.Controllers.V1.Ingredients
{
    [ApiVersion("1.0")]
    [ApiController, Route("/api/v{version:apiVersion}/ingredient-units")]
    [Authorize]
    public class IngredientUnitsController : Controller
    {
        private readonly IIngredientUnitService _service;

        public IngredientUnitsController(IIngredientUnitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<PageResult<GetAllIngredientUnitDto>> GetAll(
            [FromQuery] string searchText,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string sort
        )
        {
            var sortParser = new UriSortParser();
            var sortExpression = sort == null ? null : sortParser.Parse<GetAllIngredientUnitDto>(sort);
            var pagination = limit.HasValue && offset.HasValue ? Pagination.Of(offset.Value + 1, limit.Value) : null;
            return await _service.GetAll(searchText, pagination, sortExpression);
        }
    }
}