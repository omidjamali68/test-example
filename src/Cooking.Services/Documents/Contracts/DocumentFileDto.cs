namespace Cooking.Services.Documents.Contracts
{
    public class DocumentFileDto
    {
        public byte[] FileStream { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
    }
}