using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.StateServices.Nationalities.Contracts;
using Cooking.TestTools.StateTestTools;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Cooking.Services.Tests.Unit.StateTests.Nationalities
{
    public class NationalityServiceTests
    {
        private readonly EFInMemoryDatabase database;
        private readonly EFDataContext _context;
        private readonly INationalityService _sut;

        public NationalityServiceTests()
        {
            database = new EFInMemoryDatabase();
            _context = database.CreateDataContext<EFDataContext>();
            _sut = NationalityFactory.CreateService(_context);
        }

        [Fact]
        private async Task Get_get_all_nationalities_properly()
        {
            var firstNationality = new NationalityBuilder()
                .WithName("Iran")
                .Build(_context);
            var secondNationality = new NationalityBuilder()
                .WithName("USA")
                .Build(_context);
            
            var expected = await _sut.GetAll(null, null, null);

            expected.TotalElements.Should().Be(2);
            expected.Elements.Should().Contain(_ => _.Id == firstNationality.Id &&
                                                    _.Name == firstNationality.Name);
            expected.Elements.Should().Contain(_ => _.Id == secondNationality.Id &&
                                                    _.Name == secondNationality.Name);
        }

        [Fact]
        private async Task Get_get_all_nationalities_with_searchText_properly()
        {
            var firstNationality = new NationalityBuilder()
                .WithName("Iran")
                .Build(_context);
            var secondNationality = new NationalityBuilder()
                .WithName("USA")
                .Build(_context);
            
            var expected = await _sut.GetAll("Ira", null, null);

            expected.TotalElements.Should().Be(1);
            expected.Elements.Should().Contain(_ => _.Id == firstNationality.Id &&
                                                    _.Name == firstNationality.Name);
        }
    }
}