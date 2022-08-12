using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestExample.Services.Masters.Contracts;

namespace TestExample.RestApi.Controllers.V1.Masters
{
    [Route("/api/v{version:apiVersion}/masters")]
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