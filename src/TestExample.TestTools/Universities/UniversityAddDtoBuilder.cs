using TestExample.Services.Universities.Contracts;

namespace TestExample.TestTools.Universities
{
    public class UniversityAddDtoBuilder
    {
        private readonly AddUniversityDto _dto;

        public UniversityAddDtoBuilder()
        {
            _dto = new AddUniversityDto
            {
                Address = "dummy_address",
                Email = "dummy_email",
                Name = "dummy_name"
            };
        }

        public UniversityAddDtoBuilder WithName(string name)
        {
            _dto.Name = name;
            return this;
        }

        public UniversityAddDtoBuilder WithAddress(string address)
        {
            _dto.Address = address;
            return this;
        }

        public UniversityAddDtoBuilder WithEmail(string email)
        {
            _dto.Email = email;
            return this;
        }

        public AddUniversityDto Build()
        {
            return _dto;
        }
    }
}