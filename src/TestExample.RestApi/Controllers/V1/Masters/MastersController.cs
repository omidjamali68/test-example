using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestExample.Infrastructure.Application;
using TestExample.Services.Masters.Contracts;

namespace TestExample.RestApi.Controllers.V1.Masters
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/masters")]
    [Authorize(Roles = UserRoles.Admin)]
    public class MastersController : Controller
    {
        private readonly IMasterService _service;

        public MastersController(IMasterService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<int> Add(AddMasterDto dto)
        {
            return await _service.Add(dto);
        }
    }
}