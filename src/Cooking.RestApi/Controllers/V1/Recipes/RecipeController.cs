using System.Threading.Tasks;
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
    }
}