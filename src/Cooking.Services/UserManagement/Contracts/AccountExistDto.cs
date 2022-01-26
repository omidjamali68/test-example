using Cooking.Infrastructure.Application.Validations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class AccountExistDto
    {
        [MobileNumber] [Required] public string MobileNumber { get; set; }
    }
}