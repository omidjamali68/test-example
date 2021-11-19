using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public class AddStepOperationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid AvatarId { get; set; }
        [Required]
        public string Extension { get; set; }
    }
}
