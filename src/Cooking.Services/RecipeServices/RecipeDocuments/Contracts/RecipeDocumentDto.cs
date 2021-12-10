using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.RecipeDocuments.Contracts
{
    public class RecipeDocumentDto
    {
        public Guid DocumentId { get; set; }
        public string Extension { get; set; }
    }
}