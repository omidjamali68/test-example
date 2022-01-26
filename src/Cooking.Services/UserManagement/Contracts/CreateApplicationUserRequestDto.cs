using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class CreateApplicationUserRequestDto
    {
        [Cooking.Infrastructure.Application.Validations.MobileNumber]
        [Required] 
        public string PhoneNumber { get; set; }
        [Required] 
        public string RoleName { get; set; }
        // todo: Add Email Authentication service later 
        [MaxLength(80)]
        public string Email { get; set; }
    }
}