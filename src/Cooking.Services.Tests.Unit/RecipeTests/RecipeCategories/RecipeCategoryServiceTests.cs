using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.RecipeCtegories.Contracts;
using Cooking.TestTools.RecipeTestTools.RecipeCategories;
using FluentAssertions;
using Xunit;

namespace Cooking.Services.Tests.Unit.RecipeTests.RecipeCategories
{
    public class RecipeCategoryServiceTests
    {
        private readonly IRecipeCategoryService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;   

        public RecipeCategoryServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = RecipeCategoryFactory.CreateService(_context);
        }

        [Fact]
        private async Task Get_get_all_recipeCategories_properly()
        {
            var firstCategory = new RecipeCategoryBuilder()
                .WithTitle("FastFood")
                .Build(_context);
            var secondCategory = new RecipeCategoryBuilder()
                .WithTitle("کباب")
                .Build(_context);

            var expected = await _sut.GetAll(null);

            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Id == firstCategory.Id &&
                                           _.Title == firstCategory.Title);
            expected.Should().Contain(_ => _.Id == secondCategory.Id &&
                                           _.Title == secondCategory.Title);
        }

        [Fact]
        private async Task Get_get_all_recipeCategories_with_searchText_properly()
        {
            var firstCategory = new RecipeCategoryBuilder()
                .WithTitle("FastFood")
                .Build(_context);
            var secondCategory = new RecipeCategoryBuilder()
                .WithTitle("کباب")
                .Build(_context);

            var expected = await _sut.GetAll("کبا");

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Id == secondCategory.Id &&
                                           _.Title == secondCategory.Title);
        }
    }
}