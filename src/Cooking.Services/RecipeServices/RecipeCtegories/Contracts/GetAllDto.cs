using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.RecipeCtegories.Contracts
{
    public class GetAllRecipeCategoryDto
    {
        public int Id { get; set; }
        public string Title {  get; set; }
    }
}