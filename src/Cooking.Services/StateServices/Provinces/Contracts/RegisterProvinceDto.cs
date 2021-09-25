using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.StateServices.Provinces.Contracts
{
    public class RegisterProvinceDto
    {
        [Required] public string Title { get; set; }
    }
}