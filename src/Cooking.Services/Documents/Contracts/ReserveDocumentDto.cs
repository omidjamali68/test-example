using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.Documents.Contracts
{
    public class ReserveDocumentDto
    {
        [Required] public byte[] FileStream { get; set; }

        [Required] public string FileExtension { get; set; }
    }
}