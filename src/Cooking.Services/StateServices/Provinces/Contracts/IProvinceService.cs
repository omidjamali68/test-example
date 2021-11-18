using System.Collections.Generic;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.StateServices.Provinces.Contracts
{
    public interface IProvinceService : IService
    {
        void Register(RegisterProvinceDto dto);
        IList<GetAllProvincesDto> GetAll();
        IList<GetProvinceCitiesDto> GetCities(int id);
    }
}