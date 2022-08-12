using System.Collections.Generic;
using TestExample.Entities.Masters;

namespace TestExample.Entities.Universities
{
    public class University
    {
        public University(
            string name,
            string address,
            string email)
        {
            Name = name;
            Address = address;
            Email = email;
            Masters = new HashSet<Master>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public HashSet<Master> Masters { get; set; }
    }
}