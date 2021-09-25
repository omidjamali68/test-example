using System;

namespace Cooking.Services.UserManagement.Contracts
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string CountryCallingCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}