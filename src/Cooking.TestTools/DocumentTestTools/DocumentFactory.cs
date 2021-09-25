using System;
using Cooking.Entities.Documents;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.DocumentTestTools
{
    public class DocumentFactory
    {
        public static Document CreateDocument(EFDataContext context, DocumentStatus status)
        {
            var document = new DocumentBuilder().Build();
            document.Status = status;
            context.Manipulate(_ => _.Documents.Add(document));
            return document;
        }

    }
}