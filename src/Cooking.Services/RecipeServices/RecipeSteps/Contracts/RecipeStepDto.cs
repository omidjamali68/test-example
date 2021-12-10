namespace Cooking.Services.RecipeServices.RecipeSteps.Contracts
{
    public class RecipeStepDto
    {
        public short Order { get; set; }
        public string Description { get; set; }
        public long StepOperationId { get; set; }
    }
}