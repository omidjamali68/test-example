using Cooking.Entities.Documents;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Services.Tests.Unit.IngredientTests.Ingredients
{
    public class IngredientServiceTests
    {
        private readonly IIngredientService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        public IngredientServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = IngredientFactory.CreateService(_context);
        }

        [Fact]
        private async Task Add_add_ingredient_properly()
        {
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var dto = IngredientFactory.GenerateAddIngredientDto(
                ingredientUnitId: ingredientUnit.Id,
                title: "تخم مرغ",
                avatarId: document.Id);

            var ingredientId = await _sut.AddAsync(dto);

            var expected = await _readContext.Ingredients
               .SingleOrDefaultAsync(_ => _.Id == ingredientId);
            expected.IngredientUnitId.Should().Be(dto.IngredientUnitId);
            expected.Title.Should().Be(dto.Title);
            expected.AvatarId.Should().Be(dto.AvatarId);
            expected.Extension.Should().Be(dto.Extension);
        }
    }
}
