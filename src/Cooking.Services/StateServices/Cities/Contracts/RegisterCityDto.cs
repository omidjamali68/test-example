using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.StateServices.Cities.Contracts
{
    public class RegisterCityDto
    {
        [Required] public int ProvinceId { get; set; }

        [Required] public string Title { get; set; }
    }
}