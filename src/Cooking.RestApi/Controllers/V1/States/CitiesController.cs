using Cooking.Services.StateServices.Cities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cooking.RestApi.Controllers.V1.States
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/cities")]

    public class CitiesController : Controller
    {
        private readonly ICityService _service;

        public CitiesController(ICityService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public void Register([FromBody] RegisterCityDto dto)
        {
            _service.Register(dto);
        }

        [HttpGet("{id}")]
        public FindCityByIdDto? FindById([FromRoute] int id)
        {
            return _service.FindById(id);
        }
    }
}
