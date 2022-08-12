using System;
using TestExample.Services.Universities.Contracts;

namespace TestExample.TestTools.Universities
{
    public class UniversityUpdateDtoBuilder
    {
        private readonly UpdateUniversityDto _dto;

        public UniversityUpdateDtoBuilder()
        {
            _dto = new UpdateUniversityDto
            {
                Name = "dummy_name",
                Address = "dummy_address",
                Email = "dummy_email"
            };
        }

        public UniversityUpdateDtoBuilder WithName(string name)
        {
            _dto.Name = name;
            return this;
        }

        public UpdateUniversityDto Build()
        {
            return _dto;
        }
    }
}