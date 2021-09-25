using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.Documents.Contracts
{
    public interface DocumentRepository : Repository
    {
        Task<DocumentFileDto> GetDocumentFileById(long id);
        Task RegisterDocuments(List<long> ids);
        Task RegisterDocument(long id);
        Task RemoveDocument(long id);
        Task RemoveDocuments(List<long> ids);
        Task<bool> IsDocumentExist(long documentId);
        Task<string> GetExtension(long id);
        Task<IList<Document>> FindByIds(IEnumerable<long> ids);
        void AddDocument(Document document, byte[] fileStream, string fileExtension);
    }
}