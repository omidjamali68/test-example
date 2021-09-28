using System.Collections.Generic;

namespace Cooking.Entities.States
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
