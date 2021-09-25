using System.Collections.Generic;
using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.StatePersistence.Cities;
using Cooking.Persistence.EF.StatePersistence.Provinces;
using Cooking.Services.StateServices.Cities;
using Cooking.Services.StateServices.Cities.Contracts;
using Cooking.Services.StateServices.Provinces;
using Cooking.Services.StateServices.Provinces.Contracts;
using FluentAssertions;
using Xunit;

namespace Cooking.Services.Tests.Unit.Cities
{
    public class CityServiceTests
    {
        public CityServiceTests()
        {
            database = new EFInMemoryDatabase();
            dataContext = database.CreateDataContext<EFDataContext>();
            repository = new EFCityRepository(dataContext);
            unitOfWork = new EFUnitOfWork(dataContext);
            sut = new CityAppService(repository, unitOfWork);
            var queryRepository = new EFProvinceRepository(dataContext);
            queryService = new ProvinceAppService(queryRepository, unitOfWork);
        }

        private readonly EFInMemoryDatabase database;
        private readonly EFDataContext dataContext;
        private readonly CityRepository repository;
        private readonly UnitOfWork unitOfWork;
        private readonly CityService sut;
        private readonly ProvinceService queryService;

        [Fact]
        private void FindById_finds_city_by_id_properly()
        {
            var city = new City {Title = "shiraz"};
            dataContext.Manipulate(db =>
            {
                var province = new Province
                {
                    Title = "fars",
                    Cities = new HashSet<City>(new[] {city})
                };
                db.Provinces.Add(province);
            });

            var actualResult = sut.FindById(city.Id);

            actualResult.Should().BeEquivalentTo(new
            {
                city.Id,
                city.Title,
                city.ProvinceId
            });
        }

        [Fact]
        private void Register_registers_a_City_properly()
        {
            var province = new Province {Title = "dummy"};
            dataContext.Manipulate(db => { db.Provinces.Add(province); });

            var dto = new RegisterCityDto
            {
                Title = "dummy",
                ProvinceId = province.Id
            };
            sut.Register(dto);

            var cities = queryService.GetCities(province.Id);
            cities.Should().HaveCount(1).And.Contain(_ => _.Title == "dummy");
        }
    }
}