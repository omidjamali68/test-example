using Cooking.Services.StateServices.Provinces.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cooking.RestApi.Controllers.V1.States
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/provinces")]

    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _service;

        public ProvincesController(IProvinceService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public void Register([FromBody] RegisterProvinceDto dto)
        {
            _service.Register(dto);
        }

        [HttpGet]
        public IList<GetAllProvincesDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}/cities")]
        public IList<GetProvinceCitiesDto> GetCities([FromRoute] int id)
        {
            return _service.GetCities(id);
        }
    }
}
