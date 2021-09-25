using System.Collections.Generic;

namespace Cooking.Entities.States
{
    public class Province
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public HashSet<City> Cities { get; set; }
    }
}