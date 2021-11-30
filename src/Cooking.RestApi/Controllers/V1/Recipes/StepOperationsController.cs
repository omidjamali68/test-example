using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Web;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cooking.RestApi.Controllers.V1.Recipes
{
    [ApiVersion("1.0")]
    [ApiController, Route("/api/v{version:apiVersion}/step-operations")]
    [Authorize]
    public class StepOperationsController : Controller
    {
        private readonly IStepOperationService _service;

        public StepOperationsController(IStepOperationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<long> Add(AddStepOperationDto dto)
        {
            return await _service.AddAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(long id, UpdateStepOperationDto dto)
        {
            await _service.UpdateAsync(dto, id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _service.Delete(id);
        }

        [HttpGet("{id}")]
        public async Task<GetStepOperationDto> FindById(long id)
        {
            return await _service.GetStepOperation(id);
        }

        [HttpGet]
        public async Task<PageResult<GetAllStepOperationDto>> GetAll(
            [FromQuery] string searchText,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string sort
            )
        {
            var sortParser = new UriSortParser();
            var sortExpression = sort == null ? null : sortParser.Parse<GetAllStepOperationDto>(sort);
            var pagination = limit.HasValue && offset.HasValue ? Pagination.Of(offset.Value + 1, limit.Value) : null;
            return await _service.GetAllStepOperation(searchText, pagination, sortExpression);
        }
    }
}
