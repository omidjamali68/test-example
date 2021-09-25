using System.Linq;
using Cooking.Entities.States;
using Cooking.Services.StateServices.Cities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.StatePersistence.Cities
{
    public class EFCityRepository : CityRepository
    {
        private readonly EFDataContext _dataContext;
        private readonly DbSet<City> _set;

        public EFCityRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
            _set = _dataContext.Cities;
        }

        public void Add(City city)
        {
            _set.Add(city);
        }

        public FindCityByIdDto? FindById(int id)
        {
            return _set.Where(_ => _.Id == id).Select(_ => new FindCityByIdDto
            {
                Id = _.Id,
                Title = _.Title,
                ProvinceId = _.ProvinceId
            }).FirstOrDefault();
        }
    }
}