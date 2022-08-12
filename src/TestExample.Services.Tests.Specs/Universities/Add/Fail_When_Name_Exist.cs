using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Entities.Universities;
using TestExample.Infrastructure.Test;
using TestExample.Persistence.EF;
using TestExample.Services.Universities.Contracts;
using TestExample.Services.Universities.Exceptions;
using TestExample.Specs;
using TestExample.Specs.Infrastructure;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Specs.Universities.Add
{
    [Scenario("جلوگیری از ثبت دانشگاه با نام تکراری")]
    public class Fail_When_Name_Exist : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly IUniversityService _sut;
        private Func<Task> _expected;
        private University _existUniversity;

        public Fail_When_Name_Exist(ConfigurationFixture configuration)
        : base(configuration)
        {
            _context = CreateDataContext();
            _sut = UniversityFactory.CreateService(_context);
        }

        [Given("یک دانشگاه با نام دانشکده هنر در فهرست دانشگاه ها وجود دارد")]
        public void Given()
        {
            _existUniversity = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(_existUniversity));
        }

        [When("یک دانشگاه جدید با نام: دانشکده هنر تعریف میکنم")]
        public async Task When()
        {
            var dto = new UniversityAddDtoBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _expected = async () => await _sut.Add(dto);
        }

        [Then("باید خطای ثبت دانشگاه با نام تکراری رخ دهد")]
        [And("باید فقط یک دانشگاه با نام: دانشکده هنر در فهرست دانشگاه ها وجود داشته باشد")]
        public async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<UniversityNameExistException>();
            var dbExpected = _context.Universities.ToList();
            dbExpected.Should().HaveCount(1);
            dbExpected.Single().Id.Should().Be(_existUniversity.Id);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => When().Wait(),
                _ => Then().Wait()
            );
        }
    }
}