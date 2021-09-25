using System;
using System.Collections.Generic;
using System.Linq;
using Cooking.Entities.States;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.StatePersistence.Provinces;
using Cooking.Services.StateServices.Provinces;
using Cooking.Services.StateServices.Provinces.Contracts;
using Cooking.Services.StateServices.Provinces.Exceptions;
using FluentAssertions;
using Xunit;

namespace Cooking.Services.Tests.Unit.Provinces
{
    public class ProvinceServiceTests
    {
        public ProvinceServiceTests()
        {
            database = new EFInMemoryDatabase();
            dataContext = database.CreateDataContext<EFDataContext>();
            repository = new EFProvinceRepository(dataContext);
            unitOfWork = new EFUnitOfWork(dataContext);
            sut = new ProvinceAppService(repository, unitOfWork);
            queryService = new ProvinceAppService(repository, unitOfWork);
        }

        private readonly EFInMemoryDatabase database;
        private readonly EFDataContext dataContext;
        private readonly ProvinceRepository repository;
        private readonly UnitOfWork unitOfWork;
        private readonly ProvinceService sut;
        private readonly ProvinceService queryService;

        [Fact]
        private void GetAll_retrieves_all_provinces_properly()
        {
            dataContext.Manipulate(db =>
            {
                db.Provinces.AddRange(new List<Province>
                {
                    new Province {Title = "tehran"},
                    new Province {Title = "fars"}
                });
            });
            var expectedResult = new[]
            {
                new {Title = "tehran"},
                new {Title = "fars"}
            };

            var actualResult = sut.GetAll();

            actualResult.Select(_ => new {_.Title}).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        private void GetCities_retrieves_cities_of_a_province()
        {
            var province = new Province {Title = "dummy"};
            dataContext.Manipulate(db =>
            {
                db.Provinces.Add(province);
                db.Cities.Add(new City {Title = "tehran", Province = province});
                db.Cities.Add(new City {Title = "fars", Province = province});
            });

            var actualResult = sut.GetCities(province.Id);

            actualResult.Select(_ => new {_.Title}).Should().HaveCount(2);
            //actualResult.Should().BeEquivalentTo(new {Title = "tehran"}, new {Title = "fars"});
        }

        [Fact]
        private void Register_registers_a_Province_properly()
        {
            var dto = new RegisterProvinceDto
            {
                Title = "dummy"
            };
            sut.Register(dto);

            var provinces = queryService.GetAll();
            provinces.Should().HaveCount(1).And.Contain(_ => _.Title == "dummy");
        }

        [Fact]
        private void Register_throws_exception_when_duplicate_name_passed()
        {
            dataContext.Manipulate(db => { db.Provinces.Add(new Province {Title = "fars"}); });

            var dto = new RegisterProvinceDto
            {
                Title = "fars"
            };
            Action action = () => sut.Register(dto);

            action.Should().Throw<DuplicateProvinceNameException>();
        }
    }
}