using System;
using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;

namespace Cooking.TestTools.RecipeTestTools.RecipeDocuments
{
    public static class RecipeDocumentFactory
    {
        public static RecipeDocument CreateInstance(
            Guid docId,
            string extension)
        {
            return new RecipeDocument
            {
                DocumentId = docId,
                Extension = extension
            };
        }

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