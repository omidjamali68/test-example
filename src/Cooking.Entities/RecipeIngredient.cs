using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities
{
    public class RecipeIngredient
    {
        public double Quantity { get; set; }

        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public long IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
