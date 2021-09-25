using System;
using Cooking.Entities.Documents;

namespace Cooking.TestTools.DocumentTestTools
{
    public class DocumentBuilder
    {
        private readonly Document document;

        public DocumentBuilder()
        {
            document = new Document
            {
                FileExtension = ".png",
                Status = DocumentStatus.Reserved,
                FileId = Guid.NewGuid(),
                CreationDate = DateTime.Now
            };
        }

        public DocumentBuilder WithFileExtension(string fileExtension)
        {
            document.FileExtension = fileExtension;
            return this;
        }

        public DocumentBuilder WithFileId(Guid guid)
        {
            document.FileId = guid;
            return this;
        }

        public DocumentBuilder WithStatus(DocumentStatus status)
        {
            document.Status = status;
            return this;
        }

        public Document Build()
        {
            return document;
        }
    }
}