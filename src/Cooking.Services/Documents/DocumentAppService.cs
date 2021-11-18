using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Services.Documents.Contracts;
using Cooking.Services.Documents.Exceptions;

namespace Cooking.Services.Documents
{
    public class DocumentAppService : IDocumentService
    {
        private readonly IDocumentRepository _repository;
        private readonly IDateTimeService _dateTime;
        private readonly IUnitOfWork _unitOfWork;
        public DocumentAppService(
            IDocumentRepository repository,
            IDateTimeService dateTime,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _dateTime = dateTime;
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDto> GetDocumentsById(Guid id)
        {
            var document = await _repository.GetDocumentById(id);
            if (document == null)
                throw new DocumentNotFoundException();
            return document;
        }

        public async Task<Guid> Add(CreateDocumentDto dto)
        {
            var id = Guid.NewGuid();

            var document = new Document
            {
                Id = id,
                CreationDate = _dateTime.Now,
                Data = dto.Data,
                FileName = id.ToString("N"),
                Status = DocumentStatus.Reserve,
                Extension = dto.Extension.TrimStart('.')
            };

            _repository.AddDocument(document);

            await _unitOfWork.CompleteAsync();

            return document.Id;
        }

        public async Task DeleteDocuments(IList<Guid> documentIds)
        {
            await _repository.DeleteByIds(documentIds);

            await _unitOfWork.CompleteAsync();
        }
    }
}