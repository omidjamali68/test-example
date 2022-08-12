using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Persistence.EF;
using TestExample.Services.Universities.Contracts;
using TestExample.Specs;
using TestExample.Specs.Infrastructure;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Specs.Universities.Add
{
    [Scenario("ثبت دانشگاه")]

    public class Add_University_Properly : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly IUniversityService _sut;
        private AddUniversityDto _dto;
        private int _universitId;

        public Add_University_Properly(ConfigurationFixture configuration)
        : base(configuration)
        {
            _context = CreateDataContext();
            _sut = UniversityFactory.CreateService(_context);
        }

        [Given("هیچ دانشگاهی در فهرست دانشگاه ها وجود ندارد")]
        public void Given()
        {
        }

        [When("یک دانشگاه با نام: دانشکده هنر، ایمیل:" +
            "test@gmail.com ، آدرس: خیابان ارم تعریف میکنم")]
        public async Task When()
        {
            _dto = new UniversityAddDtoBuilder()
                .WithName("دانشکده هنر")
                .WithAddress("خیابان ارم")
                .WithEmail("test@gmail.com")
                .Build();
            _universitId = await _sut.Add(_dto);
        }

        [Then("باید فقط یک یک دانشگاه با نام: دانشکده هنر، ایمیل:" +
        "test@gmail.com ، آدرس: خیابان ارم  در فهرست دانشگاه ها وجود داشته باشد")]
        public void Then()
        {
            var expected = _context.Universities.FirstOrDefault(_ =>
                _.Id == _universitId);
            expected.Address.Should().Be(_dto.Address);
            expected.Name.Should().Be(_dto.Name);
            expected.Email.Should().Be(_dto.Email);
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