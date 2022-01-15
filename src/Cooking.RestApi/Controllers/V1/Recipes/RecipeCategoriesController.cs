using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Services.RecipeServices.RecipeCtegories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Cooking.RestApi.Controllers.V1.Recipes
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/recipe-categories")]
    public class RecipeCategoriesController : Controller
    {
        private readonly IRecipeCategoryService _service;

        public RecipeCategoriesController(IRecipeCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IList<GetAllRecipeCategoryDto>> GetAll([FromQuery] string searchText)
        {
            return await _service.GetAll(searchText);
        }
    }
}