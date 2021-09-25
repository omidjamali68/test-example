using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities
{
    public class StepOperation
    {
        public StepOperation()
        {
            RecipeSteps = new HashSet<RecipeStep>();
        }

        public long Id { get; set; }
        public string Title {  get; set; }

        public Guid AvatarId { get; set; }
        public string Extension { get; set; }
        public HashSet<RecipeStep> RecipeSteps { get; set; }
    }
}
