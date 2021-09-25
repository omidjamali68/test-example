using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities
{
    public class Nationality
    {
        public Nationality()
        {
            Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<Recipe> Recipes { get; set; }
    }
}
