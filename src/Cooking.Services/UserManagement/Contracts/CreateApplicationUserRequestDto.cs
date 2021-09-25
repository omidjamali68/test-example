using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class CreateApplicationUserRequestDto
    {
        [Required] 
        public string NationalCode { get; set; }
        [Required] 
        public string MobileNumber { get; set; }
        [Required] 
        public string RoleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        [MaxLength(80)]
        public string Email { get; set; }
    }
}