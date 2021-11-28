using Cooking.Entities.Recipes;

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
    }
}
