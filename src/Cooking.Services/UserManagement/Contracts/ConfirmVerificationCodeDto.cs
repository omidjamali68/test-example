using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class ConfirmVerificationCodeDto
    {
        [Required] public string NationalCode { get; set; }

        [Required] public string LanguageCode { get; set; }

        [Required] public uint VerificationCode { get; set; }

        [Required] public string ZoneName { get; set; }

        [Required] public string roleName { get; set; }
    }
}