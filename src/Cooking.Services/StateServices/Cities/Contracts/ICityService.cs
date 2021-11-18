using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Cities.Contracts
{
    public interface ICityService : IService
    {
        void Register(RegisterCityDto dto);
        FindCityByIdDto? FindById(int id);
    }
}