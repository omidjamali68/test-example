using Cooking.Entities.CommonEntities;
using Cooking.Infrastructure.Application.Validations;

namespace Cooking.Services.Documents.Contracts
{
    public class DocumentInfoDto
    {
        [Required(AllowDefaultValues = false)] public DocumentType Type { get; set; }

        public string Description { get; set; }
    }
}