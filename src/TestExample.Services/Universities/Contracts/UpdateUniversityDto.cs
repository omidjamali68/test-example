using System.ComponentModel.DataAnnotations;

namespace TestExample.Services.Universities.Contracts
{
    public class UpdateUniversityDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}