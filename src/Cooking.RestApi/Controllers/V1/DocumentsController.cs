using Cooking.Infrastructure;
using Cooking.Services.Documents.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeMapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Cooking.RestApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController, Route("api/v{version:apiVersion}/documents")]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _service;
        private readonly IImagingService _imagingService;

        public DocumentsController(IDocumentService service, IImagingService imagingService)
        {
            _service = service;
            _imagingService = imagingService;
        }

        [HttpPost]
        public async Task<Guid> Add([FromForm, Required] IFormFile file)
        {
            var documentDto = new CreateDocumentDto
            {
                Extension = Path.GetExtension(file.FileName),
                Data = await FormFileToByteArrayAsync(file)
            };
            return await _service.Add(documentDto);
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Download([FromRoute, Required] Guid id,
                                                        [FromQuery] int? size)
        {
            var file = await _service.GetDocumentsById(id);
            var data = file.Data;

            if (size.HasValue)
            {
                data = _imagingService.GetThumbnail(file.Data, size.Value);
            }
            return File(data, MimeUtility.GetMimeMapping(file.Extension));
        }

        private async Task<byte[]> FormFileToByteArrayAsync(IFormFile file)
        {
            byte[] fileStream;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileStream = memoryStream.ToArray();
                await memoryStream.FlushAsync();
            }

            return fileStream;
        }
    }
}
