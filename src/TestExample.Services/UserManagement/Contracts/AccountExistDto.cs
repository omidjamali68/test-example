using TestExample.Infrastructure.Application.Validations;

namespace TestExample.Services.UserManagement.Contracts
{
    public class AccountExistDto
    {
        [Required] public string NationalCode { get; set; }
    }
}