using Cooking.Entities.Documents;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.Services.IngredientServices.Ingredients.Exceptions;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.Ingredients;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        [Fact]
        private async Task Add_fail_when_title_and_unit_exist()
        {
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var dto = IngredientFactory.GenerateAddIngredientDto(
                ingredientUnitId: ingredientUnit.Id,
                title: "تخم مرغ",
                avatarId: doc.Id);

            Func<Task> expected = async () => await _sut.AddAsync(dto);

            await expected.Should().ThrowExactlyAsync<IngredientTitleAndUnitExistException>();
            var dbExpected = _readContext.Ingredients
                .Where(_ => _.Title.Equals(dto.Title) && _.IngredientUnitId == dto.IngredientUnitId);
            dbExpected.Should().HaveCount(1);
        }

        [Fact]
        private async Task Update_update_ingredient_properly()
        {
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredient = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var dto = IngredientFactory.GenerateUpdateIngredientDto(
                ingredientUnitId: ingredientUnit.Id,
                title: "گوجه فرنگی",
                avatarId: doc.Id);

            await _sut.UpdateAsync(ingredient.Id, dto);

            var expected = await _readContext.Ingredients
               .SingleOrDefaultAsync(_ => _.Id == ingredient.Id);
            expected.IngredientUnitId.Should().Be(dto.IngredientUnitId);
            expected.Title.Should().Be(dto.Title);
            expected.AvatarId.Should().Be(dto.AvatarId);
            expected.Extension.Should().Be(dto.Extension);
        } 
        
        [Fact]
        private async Task Update_fail_when_title_and_unit_exist()
        {
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredient_egg = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);
            var document_2 = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            new IngredientBuilder(ingredientUnit.Id, document_2)
                .WithTitle("گوجه فرنگی")
                .Build(_context);
            var doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var dto = IngredientFactory.GenerateUpdateIngredientDto(
                ingredientUnitId: ingredientUnit.Id,
                title: "گوجه فرنگی",
                avatarId: doc.Id);

            Func<Task> expected = async () => await _sut.UpdateAsync(ingredient_egg.Id, dto);

            await expected.Should().ThrowExactlyAsync<IngredientTitleAndUnitExistException>();
            var dbExpected = _readContext.Ingredients
                .Where(_ => _.Title.Equals(dto.Title) && _.IngredientUnitId == dto.IngredientUnitId);
            dbExpected.Should().HaveCount(1);
        }

        [Fact]
        private async Task Delete_remove_ingredient_properly()
        {
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
            var document = DocumentFactory.CreateDocument(_context, DocumentStatus.Register);
            var ingredient = new IngredientBuilder(ingredientUnit.Id, document)
                .WithTitle("تخم مرغ")
                .Build(_context);

            await _sut.DeleteAsync(ingredient.Id);

            var expected = await _readContext.Ingredients
               .Where(_ => _.Id == ingredient.Id)
               .ToListAsync();
            expected.Should().BeNullOrEmpty();
        }
    }
}
