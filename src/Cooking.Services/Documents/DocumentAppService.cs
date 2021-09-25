using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Services.Documents.Contracts;

namespace Cooking.Services.Documents
{
    public class DocumentAppService : DocumentService
    {
        private readonly DateTimeService _dateTimeService;
        private readonly DocumentRepository _documentRepository;
        private readonly UnitOfWork _unitOfWork;

        public DocumentAppService(
            DocumentRepository documentRepository,
            DateTimeService dateTimeService,
            UnitOfWork unitOfWork
        )
        {
            _documentRepository = documentRepository;
            _dateTimeService = dateTimeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentFileDto> GetDocumentsFileById(long id)
        {
            return await _documentRepository.GetDocumentFileById(id);
        }

        public async Task<long> ReserveDocument(ReserveDocumentDto dto)
        {
            var document = new Document
            {
                Status = DocumentStatus.Reserved,
                CreationDate = _dateTimeService.Now
            };

            _documentRepository.AddDocument(document, dto.FileStream, dto.FileExtension);

            await _unitOfWork.Complete();

            return document.Id;
        }
    }
}