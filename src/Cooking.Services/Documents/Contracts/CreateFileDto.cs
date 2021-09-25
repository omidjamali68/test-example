namespace Cooking.Services.Documents.Contracts
{
    public class CreateFileDto
    {
        public byte[] FileStream { get; set; }
        public string FileExtension { get; set; }
    }
}