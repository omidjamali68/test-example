using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cooking.Services.Ingredients.Ingredients.Contracts
{
    public class AddIngredientDto
    {
        [Required]
        public int IngredientUnitId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Guid AvatarId { get; set; }

        [Required]
        public string Extension { get; set; }
    }
}
