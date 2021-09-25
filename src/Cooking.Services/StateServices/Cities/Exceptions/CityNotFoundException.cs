using Cooking.Infrastructure.Entities;

namespace Cooking.Services.StateServices.Cities.Exceptions
{
    public class CityNotFoundException : BusinessException
    {
        public int Id { get; set; }
    }
}