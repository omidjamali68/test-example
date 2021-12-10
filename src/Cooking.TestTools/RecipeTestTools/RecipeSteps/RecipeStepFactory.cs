using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;

namespace Cooking.TestTools.RecipeTestTools.RecipeSteps
{
    public static class RecipeStepFactory
    {
        public static RecipeStep CreateInstance(long stepOperationId)
        {
            return new RecipeStep
            {
                StepOperationId = stepOperationId,
                Description = "dummy_desc"
            };
        }

        public static RecipeStepDto GenerateDto(
            long stepOperationId,
            string description = "dummy_desc",
            short order = 1
        )
        {
            return new RecipeStepDto{
                Description = description,
                Order = order,
                StepOperationId= stepOperationId
            };
        }
    }
}
