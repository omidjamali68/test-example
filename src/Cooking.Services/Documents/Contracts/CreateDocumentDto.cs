using Cooking.Entities.CommonEntities;

namespace Cooking.Services.Documents.Contracts
{
    public class CreateDocumentDto
    {
        public byte[] Data { get; set; }
        public string Extension { get; set; }
    }
}