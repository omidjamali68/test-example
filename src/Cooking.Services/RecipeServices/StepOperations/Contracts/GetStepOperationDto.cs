using System;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public class GetStepOperationDto
    {
        public string Title { get; set; }

        public Guid AvatarId { get; set; }
        public string Extension { get; set; }
    }
}
