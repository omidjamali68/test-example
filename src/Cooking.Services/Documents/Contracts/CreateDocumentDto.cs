using Cooking.Entities.CommonEntities;

namespace Cooking.Services.Documents.Contracts
{
    public class CreateDocumentDto
    {
        public string Description { get; set; }

        public DocumentType Type { get; set; }
        public CreateFileDto CreateFileDto { get; set; }
    }
}