using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Entities.Universities;
using TestExample.Infrastructure.Test;
using TestExample.Persistence.EF;
using TestExample.Services.Universities.Contracts;
using TestExample.Specs;
using TestExample.Specs.Infrastructure;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Specs.Universities.Update
{
    [Scenario("ویرایش دانشگاه")]
    public class Update_University_Properly : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly IUniversityService _sut;
        private University _university;
        private UpdateUniversityDto _dto;

        public Update_University_Properly(ConfigurationFixture configuration)
        : base(configuration)
        {
            _context = CreateDataContext();
            _sut = UniversityFactory.CreateService(_context);
        }

        [Given("یک دانشگاه با نام دانشکده هنر در فهرست دانشگاه ها وجود دارد")]
        public void Given()
        {
            _university = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(_university));
        }

        [When("نام این دانشگاه را به دانشکده مهندسی تغییر میدهم")]
        public async Task When()
        {
            _dto = new UniversityUpdateDtoBuilder()
                .WithName("دانشکده مهندسی")
                .Build();
            await _sut.Update(_university.Id, _dto);
        }

        [Then("فقط باید یک دانشگاه با نام دانشکده مهندسی در فهرست دانشگاه ها وجود داشته باشد")]
        public void Then()
        {
            var expected = _context.Universities.SingleOrDefault(_ =>
                _.Id == _university.Id);
            expected.Name.Should().Be(_dto.Name);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => When().Wait(),
                _ => Then()
            );
        }
    }
}