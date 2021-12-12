using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using FluentAssertions;
using Xunit;

namespace Cooking.Services.Tests.Unit.IngredientTests.IngredientUnits
{
    public class IngredientUnitServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly IIngredientUnitService _sut;
        public IngredientUnitServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = IngredientUnitFactory.CreateService(_context);
        }

        [Theory]
        [InlineData(2, null)]
        [InlineData(1, "گرم")]
        private async Task Get_all_ingredientUnits_properly(byte totalElementsExpected ,string searchText)
        {
            var firstUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var secondUnit = new IngredientUnitBuilder()
                .WithTitle("گرم'")
                .Build(_context);

            var expected = await _sut.GetAll(searchText, null, null);

            expected.TotalElements.Should().Be(totalElementsExpected);
        }
    }
}