using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.Documents.Contracts
{
    public interface DocumentService : Service
    {
        Task<DocumentFileDto> GetDocumentsFileById(long id);
        Task<long> ReserveDocument(ReserveDocumentDto dto);
    }
}