using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Cooking.Entities.CommonEntities;

namespace Cooking.Entities.ApplicationIdentities
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