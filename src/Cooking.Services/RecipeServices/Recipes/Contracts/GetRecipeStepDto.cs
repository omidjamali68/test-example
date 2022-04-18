using System;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class GetRecipeStepDto
    {
        public short Order { get; set; }
        public string Description { get; set; }
        public long StepOperationId { get; set; }
        public Guid AvatarId { get; set; }
    }
}