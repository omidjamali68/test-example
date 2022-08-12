using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Infrastructure.Test;
using TestExample.Infrastructure.Web;
using TestExample.Persistence.EF;
using TestExample.Services.Universities.Contracts;
using TestExample.Services.Universities.Exceptions;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Unit.Universities
{
    public class UniversityServiseTests
    {
        private readonly IUniversityService _sut;
        private readonly EFDataContext _context;

        public UniversityServiseTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _sut = UniversityFactory.CreateService(_context);
        }

        [Fact]
        public async Task Add_add_university_properly()
        {
            var dto = new UniversityAddDtoBuilder()
                .WithName("دانشکده هنر")
                .WithAddress("خیابان ارم")
                .WithEmail("test@gmail.com")
                .Build();

            var universitId = await _sut.Add(dto);

            var expected = _context.Universities.FirstOrDefault(_ =>
                _.Id == universitId);
            expected.Address.Should().Be(dto.Address);
            expected.Name.Should().Be(dto.Name);
            expected.Email.Should().Be(dto.Email);
        }

        [Fact]
        public async Task Add_throws_exception_when_name_exist()
        {
            var existUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(existUniversity));
            var dto = new UniversityAddDtoBuilder()
                .WithName("دانشکده هنر")
                .Build();

            Func<Task> expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<UniversityNameExistException>();
            var dbExpected = _context.Universities.ToList();
            dbExpected.Should().HaveCount(1);
            dbExpected.Single().Id.Should().Be(existUniversity.Id);
        }

        [Fact]
        public async Task Update_update_universities_properly()
        {
            var university = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(university));
            var dto = new UniversityUpdateDtoBuilder()
                .WithName("دانشکده مهندسی")
                .Build();

            await _sut.Update(university.Id, dto);

            var expected = _context.Universities.SingleOrDefault(_ =>
                _.Id == university.Id);
            expected.Name.Should().Be(dto.Name);
        }

        [Fact]
        public async Task Update_throws_exception_when_name_exist()
        {
            var artUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(artUniversity));
            var engineeringUniversity = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(engineeringUniversity));
            var dto = new UniversityUpdateDtoBuilder()
                .WithName("دانشکده مهندسی")
                .Build();

            Func<Task> expected = async () =>
                await _sut.Update(artUniversity.Id, dto);

            await expected.Should().ThrowExactlyAsync<UniversityNameExistException>();
            var dbExpected = _context.Universities.Single(_ =>
                _.Name == dto.Name);
            dbExpected.Id.Should().Be(engineeringUniversity.Id);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Update_throws_exception_when_university_not_found(int invalidId)
        {
            var dto = new UniversityUpdateDtoBuilder()
                .Build();

            Func<Task> expected = async () => await _sut.Update(invalidId, dto);

            await expected.Should().ThrowExactlyAsync<UniversityNotFoundException>();
        }

        [Fact]
        public async Task Get_get_university_by_id_properly()
        {
            var university = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(university));

            var expected = await _sut.GetById(university.Id);

            expected.Address.Should().Be(university.Address);
            expected.Name.Should().Be(university.Name);
            expected.Email.Should().Be(university.Email);
        }

        [Fact]
        public async Task Get_get_all_universities_properly()
        {
            var artUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(artUniversity));
            var engineeringUniversity = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(engineeringUniversity));

            var expected = await _sut.GetAll();

            expected.TotalElements.Should().Be(2);
            expected.Elements.Should().Contain(_ =>
                _.Id == artUniversity.Id &&
                _.Name == artUniversity.Name &&
                _.Address == artUniversity.Address &&
                _.Email == artUniversity.Email);
            expected.Elements.Should().Contain(_ =>
                _.Id == engineeringUniversity.Id &&
                _.Name == engineeringUniversity.Name &&
                _.Address == engineeringUniversity.Address &&
                _.Email == engineeringUniversity.Email);
        }

        [Fact]
        public async Task Get_get_all_universities_with_sort_properly()
        {
            var artUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(artUniversity));
            var engineeringUniversity = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(engineeringUniversity));
            var sortText = "+name";
            var sortParser = new UriSortParser();
            var sort = !string.IsNullOrEmpty(sortText) ?
                sortParser.Parse<GetAllUniversitiesDto>(sortText) :
                null;

            var expected = await _sut.GetAll(sort: sort);

            expected.TotalElements.Should().Be(2);
            expected.Elements.First().Id.Should().Be(engineeringUniversity.Id);
        }

        [Fact]
        public async Task Get_get_all_universities_with_search_properly()
        {
            var artUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(artUniversity));
            var engineeringUniversity = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(engineeringUniversity));

            var expected = await _sut.GetAll(search: "مهند");

            expected.TotalElements.Should().Be(1);
            expected.Elements.Single().Id.Should().Be(engineeringUniversity.Id);
        }

        [Fact]
        public async Task Delete_delete_university_properly()
        {
            var university = new UniversityBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            _context.Manipulate(_ => _.Add(university));

            await _sut.Delete(university.Id);

            var expected = _context.Universities.FirstOrDefault(_ =>
                _.Id == university.Id);
            expected.Should().BeNull();
        }

        [Theory]
        [InlineData(-1)]
        public async Task Delete_throws_exception_when_university_not_found(int invalidId)
        {
            Func<Task> expected = async () => 
                await _sut.Delete(invalidId);

            await expected.Should().ThrowExactlyAsync<UniversityNotFoundException>();
        }
    }
}