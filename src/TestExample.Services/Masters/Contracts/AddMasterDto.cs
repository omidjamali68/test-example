using TestExample.Infrastructure.Application.Validations;

namespace TestExample.Services.Masters.Contracts
{
    public class AddMasterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string NationalCode { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.MaxLength(11)]
        [MobileNumber]
        public string Mobile { get; set; }
        [Required]
        public int UniversityId { get; set; }
    }
}