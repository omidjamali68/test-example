using System.ComponentModel.DataAnnotations;

namespace TestExample.Services.UserManagement.Contracts
{
    public class SendVerificationCodeDto
    {
        [Required] public string PhoneNumber { get; set; }
    }
}