using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.IngredientServices.Ingredients.Contracts
{
    public class GetAllIngredientDto
    {
        public long Id { get; set; }
        public string IngredientUnitTitle { get; set; }
        public string Title { get; set; }
        public Guid AvatarId { get; set; }
        public string Extension { get; set; }
    }
}
