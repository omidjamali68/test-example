using Microsoft.AspNetCore.Identity;
using System;

namespace Cooking.Entities.ApplicationIdentities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime CreationDate { get; set; }
    }
}