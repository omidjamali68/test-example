using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities
{
    public class IngredientUnit
    {
        public IngredientUnit()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Title {  get; set; }

        public HashSet<Ingredient> Ingredients { get; set; }
    }
}
