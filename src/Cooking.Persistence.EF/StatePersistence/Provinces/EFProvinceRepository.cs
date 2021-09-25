using System.Collections.Generic;
using System.Linq;
using Cooking.Entities.States;
using Cooking.Services.StateServices.Provinces.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.StatePersistence.Provinces
{
    public class EFProvinceRepository : ProvinceRepository
    {
        private readonly EFDataContext _dataContext;
        private readonly DbSet<Province> _provinces;

        public EFProvinceRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
            _provinces = _dataContext.Provinces;
        }

        public void Add(Province province)
        {
            _provinces.Add(province);
        }

        public bool ExistsByName(string name)
        {
            return _provinces.Any(_ => _.Title == name);
        }

        public IList<GetAllProvincesDto> GetAllProvinces()
        {
            return _provinces.Select(_ => new GetAllProvincesDto
            {
                Id = _.Id,
                Title = _.Title
            }).ToList();
        }

        public IList<GetProvinceCitiesDto> GetCities(int id)
        {
            return _dataContext.Cities.Where(_ => _.ProvinceId == id).Select(_ => new GetProvinceCitiesDto
            {
                Id = _.Id,
                Title = _.Title
            }).ToList();
        }
    }
}