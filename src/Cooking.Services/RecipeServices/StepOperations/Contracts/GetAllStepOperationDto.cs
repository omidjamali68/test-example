using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public class GetAllStepOperationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public Guid AvatarId { get; set; }
        public string Extension { get; set; }
    }
}
