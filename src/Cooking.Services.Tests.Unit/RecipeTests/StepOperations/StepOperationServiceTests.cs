﻿using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Services.RecipeServices.StepOperations.Exceptions;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Services.Tests.Unit.RecipeTests.StepOperations
{
    public class StepOperationServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly IStepOperationService _sut;
        public StepOperationServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = StepOperationFactory.CreateService(_context);
        }

        [Fact]
        private async Task Add_stepOperation_properly()
        {
            var document = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            var dto = StepOperationFactory.GenerateAddDto("تفت دادن", document.Id);

            var addedId = await _sut.AddAsync(dto);

            var expected = _readContext.StepOperations.FirstOrDefault(_ => _.Id == addedId);
            expected.Title.Should().Be(dto.Title);
            expected.AvatarId.Should().Be(dto.AvatarId);
        }

        [Fact]
        private async Task Update_stepOperation_properly()
        {
            var avatar = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            var stepOperation = new StepOperationBuilder(avatar)
                .WithTitle("تفت دادن")
                .Build(_context);
            var dto = StepOperationFactory.GenerateUpdateDto(avatar, "سرخ کردن");

            await _sut.UpdateAsync(dto, stepOperation.Id);

            var expected = _readContext.StepOperations.First(_ => _.Id == stepOperation.Id);
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        private async Task Prevent_update_stepOperation_when_title_exist()
        {
            var avatar = DocumentFactory.CreateDocument(_context, Entities.Documents.DocumentStatus.Reserve);
            var firstStepOperation = new StepOperationBuilder(avatar)
                .WithTitle("تفت دادن")
                .Build(_context);
            var secondtStepOperation = new StepOperationBuilder(avatar)
                .WithTitle("سرخ کردن")
                .Build(_context);
            var dto = StepOperationFactory.GenerateUpdateDto(avatar, "سرخ کردن");

            Func<Task> expected = async ()=> await _sut.UpdateAsync(dto, firstStepOperation.Id);

            await expected.Should().ThrowExactlyAsync<StepOperationTitleExistException>();
            var dbExpected = _readContext.StepOperations.First(_ => _.Title == dto.Title);
            dbExpected.Should().BeEquivalentTo(secondtStepOperation);
        }
    }
}
