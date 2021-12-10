using System;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;

namespace Cooking.TestTools.RecipeTestTools.RecipeDocuments
{
    public static class RecipeDocumentFactory
    {
        public static RecipeDocumentDto GenerateDto(Guid documentId, string extension = "jpj")
        {
            return new RecipeDocumentDto
            {
                DocumentId = documentId,
                Extension = extension
            };
        }
    }
}