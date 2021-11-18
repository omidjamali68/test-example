using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.Documents.Contracts
{
    public interface IDocumentService : IService
    {
        Task<DocumentDto> GetDocumentsById(Guid id);
        Task<Guid> Add(CreateDocumentDto dto);
        Task DeleteDocuments(IList<Guid> documentIds);
    }
}