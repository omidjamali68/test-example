using System;
using Microsoft.AspNetCore.Identity;
using TestExample.Entities.CommonEntities;
using TestExample.Entities.Masters;

namespace TestExample.Entities.ApplicationIdentities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }

        public Mobile Mobile { get; set; }
        public DateTime CreationDate { get; set; }
    }
}