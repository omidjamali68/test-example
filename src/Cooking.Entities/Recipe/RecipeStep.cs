namespace Cooking.Entities.Recipe
{
    public class RecipeStep
    {
        public short Order { get; set; }
        public string Description { get; set; }

        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public long StepOperationId { get; set; }
        public StepOperation StepOperation { get; set; }
    }
}
