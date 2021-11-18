using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.Documents.Contracts
{
    public interface IDocumentRepository : IRepository
    {
        void AddDocument(Document document);
        Task RegisterDocument(Guid id);
        Task RegisterDocuments(List<Guid> ids);
        Task<DocumentDto?> GetDocumentById(Guid id);
        Task DeleteByIds(IList<Guid> documentIds);
        Task<Document> FindById(Guid id);
        Task<IList<Document>> GetAllByIds(IList<Guid> ids);
    }
}