using Cooking.Entities.Documents;
using System;
using System.Collections.Generic;

namespace Cooking.Entities.Recipes
{
    public class StepOperation
    {
        public StepOperation()
        {
            RecipeSteps = new HashSet<RecipeStep>();
        }

        public long Id { get; set; }
        public string Title {  get; set; }
        [DocumentFlag]
        public Guid AvatarId { get; set; }
        public string Extension { get; set; }
        public HashSet<RecipeStep> RecipeSteps { get; set; }
    }
}
