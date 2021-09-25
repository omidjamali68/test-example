using Cooking.Infrastructure.Application.Validations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class CreateApplicationUserDto
    {
        [Required] public string NationalCode { get; set; }

        [Required] [MobileNumber] public string MobileNumber { get; set; }

        [Required] public string CountryCallingCode { get; set; }

        [Required] public string ZoneName { get; set; }

        [Required] public string LanguageCode { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }
    }
}