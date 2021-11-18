using System.Collections.Generic;
using Cooking.Entities.States;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Provinces.Contracts
{
    public interface IProvinceRepository : IRepository
    {
        void Add(Province province);
        bool ExistsByName(string name);
        IList<GetAllProvincesDto> GetAllProvinces();
        IList<GetProvinceCitiesDto> GetCities(int id);
    }
}