using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TestExample.Infrastructure.Test;
using TestExample.Persistence.EF;
using TestExample.Services.Masters.Contracts;
using TestExample.TestTools.Masters;
using TestExample.TestTools.Universities;
using Xunit;

namespace TestExample.Services.Tests.Unit.Masters
{
    public class MastersServiceTests
    {
        private readonly IMasterService _sut;
        private readonly EFDataContext _context;

        public MastersServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _sut = MasterFactory.CreateService(_context);
        }

        [Fact]
        public async Task Add_add_master_with_user_properly()
        {
            var university = new UniversityBuilder()
                .WithName("دانشکده هنر")
                .Build();
            _context.Manipulate(_ => _.Add(university));
            var dto = new MasterAddDtoBuilder()
                .WithName("علی", "احمدی")
                .WithNationalCode("2280117777")
                .WithMobile("9177875555")
                .WithUniversity(university.Id)
                .Build();

            var masterId = await _sut.Add(dto);

            var expected = _context.Masters.FirstOrDefault(_ =>
                _.Id == masterId);
            expected.UniversityId.Should().Be(university.Id);
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.NationalCode.Should().Be(dto.NationalCode);
            expected.Mobile.Should().Be(dto.Mobile);
            expected.UserId.Should().NotBeEmpty();
        }
    }
}