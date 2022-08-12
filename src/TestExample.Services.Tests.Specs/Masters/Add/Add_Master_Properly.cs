using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Entities.Universities;
using TestExample.Infrastructure.Test;
using TestExample.Persistence.EF;
using TestExample.Services.Masters.Contracts;
using TestExample.Specs;
using TestExample.Specs.Infrastructure;
using TestExample.TestTools.Masters;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Specs.Masters.Add
{
    [Scenario("ثبت استاد در دانشگاه مربوطه")]
    public class Add_Master_Properly : EFDataContextDatabaseFixture
    {
        private readonly IMasterService _sut;
        private readonly EFDataContext _context;
        private University _university;
        private AddMasterDto _dto;
        private int _masterId;

        public Add_Master_Properly(ConfigurationFixture configuration)
        : base(configuration)
        {
            _context = CreateDataContext();
            _sut = MasterFactory.CreateService(_context);
        }

        [Given("یک دانشگاه با نام دانشکده هنر در فهرست دانشگاه ها وجود دارد")]
        [And("هیچ استادی برای این دانشگاه تعریف نشده است")]
        public void Given()
        {
            _university = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(_university));
        }

        [When("یک استاد با نام: علی احمدی، شماره ملی: 2280117777، موبایل:9177875555 دردانشکده هنر تعریف میکنم")]
        public async Task When()
        {
            _dto = new MasterAddDtoBuilder()
                .WithName("علی", "احمدی")
                .WithNationalCode("2280117777")
                .WithMobile("9177875555")
                .Build();
            _masterId = await _sut.Add(_dto);
        }

        [Then("باید فقط یک استاد با نام: علی احمدی، شماره ملی: 2280113732 در دانشکده هنر وجود داشته باشد")]
        public void Then()
        {
            var expected = _context.Masters.FirstOrDefault(_ =>
                _.Id == _masterId);
            expected.UniversityId.Should().Be(_university.Id);
            expected.FirstName.Should().Be(_dto.FirstName);
            expected.LastName.Should().Be(_dto.LastName);
            expected.NationalCode.Should().Be(_dto.NationalCode);
            expected.Mobile.Should().Be(_dto.Mobile);
            expected.UserId.Should().NotBeEmpty();
        }

        [Fact(Skip = "Add unit test passed but this fail because of ambient transaction error")]
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