using System;
using System.Collections.Generic;
using Cooking.Entities.Documents;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.DocumentTestTools
{
    public class DocumentFactory
    {
        public static Document CreateDocument(EFDataContext context, DocumentStatus status)
        {
            var fileId = Guid.NewGuid();
            var document = new Document
            {
                Id = fileId,
                CreationDate = DateTime.Now,
                Data = new byte[100],
                Extension = "jpg",
                FileName = "Pic",
                Status = status
            };

            context.Manipulate(_ => _.Set<Document>().Add(document));
            return document;
        }


    }
}