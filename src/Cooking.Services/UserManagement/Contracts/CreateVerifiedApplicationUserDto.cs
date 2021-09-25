using System;

namespace Cooking.Services.UserManagement.Contracts
{
    public class CreateVerifiedApplicationUserDto
    {
        public Guid UserId { get; set; }
        public string NationalCode { get; set; }
        public string CountryCallingCode { get; set; }
        public string MobileNumber { get; set; }
        public string LanguageCode { get; set; }
        public string ZoneName { get; set; }
        public string roleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}