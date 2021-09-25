using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Cities.Contracts
{
    public interface CityService : Service
    {
        void Register(RegisterCityDto dto);
        FindCityByIdDto? FindById(int id);
    }
}