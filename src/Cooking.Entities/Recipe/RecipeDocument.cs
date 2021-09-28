using System;

namespace Cooking.Entities.Recipe
{
    public class RecipeDocument
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Guid DocumentId { get; set; }
        public string Extension { get; set; }
    }
}
