using Cooking.Entities.States;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Cities.Contracts
{
    public interface ICityRepository : IRepository
    {
        void Add(City city);
        FindCityByIdDto? FindById(int id);
    }
}