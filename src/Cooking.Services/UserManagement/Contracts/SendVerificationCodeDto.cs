using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class SendVerificationCodeDto
    {
        [Required] public string PhoneNumber { get; set; }
    }
}