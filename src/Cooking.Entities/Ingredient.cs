using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities
{
    public class Ingredient
    {
        public Ingredient()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public Guid AvatarId { get; set; }
        public string Extension { get; set; }

        public int IngredientUnitId { get; set; }
        public IngredientUnit IngredientUnit { get; set; }
        public HashSet<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
