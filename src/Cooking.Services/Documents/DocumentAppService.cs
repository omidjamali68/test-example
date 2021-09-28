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
    public class DocumentAppService : DocumentService
    {
        private readonly DocumentRepository _repository;
        private readonly DateTimeService _dateTime;
        private readonly UnitOfWork _unitOfWork;
        public DocumentAppService
        (DocumentRepository repository,
            UnitOfWork unitOfWork, DateTimeService dateTime)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _dateTime = dateTime;
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

            await _unitOfWork.Complete();

            return document.Id;
        }

        public async Task DeleteDocuments(IList<Guid> documentIds)
        {
            await _repository.DeleteByIds(documentIds);

            await _unitOfWork.Complete();
        }
    }
}