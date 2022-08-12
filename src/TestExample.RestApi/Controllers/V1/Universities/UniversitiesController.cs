using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestExample.Infrastructure.Application;
using TestExample.Infrastructure.Web;
using TestExample.Services.Universities.Contracts;

namespace TestExample.RestApi.Controllers.V1.Universities
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/universities")]
    [Authorize(Roles = UserRoles.Admin)]
    public class UniversitiesController : Controller
    {
        private readonly IUniversityService _service;
        private readonly UriSortParser _sortParser;

        public UniversitiesController(IUniversityService service, UriSortParser sortParser)
        {
            _service = service;
            _sortParser = sortParser;
        }

        [HttpPost]
        public async Task<int> Add(AddUniversityDto dto)
        {
            return await _service.Add(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, UpdateUniversityDto dto)
        {
            await _service.Update(id, dto);
        }

        [HttpGet("{id}")]
        public async Task<GetUniversityDto> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpGet]
        public async Task<PageResult<GetAllUniversitiesDto>> GetAll(
            [FromQuery] string? sort,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] string? search)
        {
            var sortExpresion = !String.IsNullOrEmpty(sort) ?
                _sortParser.Parse<GetAllUniversitiesDto>(sort) :
                null;

            var pagination = limit.HasValue && offset.HasValue ?
                Pagination.Of(offset.Value + 1, limit.Value) :
                null;

            return await _service.GetAll(pagination, sortExpresion, search);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.Delete(id);
        }
    }
}