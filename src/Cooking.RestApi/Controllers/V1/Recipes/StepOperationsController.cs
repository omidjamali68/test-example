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
    }
}
