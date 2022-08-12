using System;
using TestExample.Services.Masters.Contracts;

namespace TestExample.TestTools.Masters
{
    public class MasterAddDtoBuilder
    {
        private readonly AddMasterDto _dto;

        public MasterAddDtoBuilder()
        {
            _dto = new AddMasterDto
            {
                FirstName = "dummy_name",
                LastName = "dummy_family",
                NationalCode = "dummy_code",
                UniversityId = 1
            };
        }

        public MasterAddDtoBuilder WithUniversity(int universityId)
        {
            _dto.UniversityId = universityId;
            return this;
        }

        public MasterAddDtoBuilder WithName(string firstName, string lastName)
        {
            _dto.FirstName = firstName;
            _dto.LastName = lastName;
            return this;
        }

        public MasterAddDtoBuilder WithNationalCode(string nationalCode)
        {
            _dto.NationalCode = nationalCode;
            return this;
        }

        public MasterAddDtoBuilder WithMobile(string mobile)
        {
            _dto.Mobile = mobile;
            return this;
        }

        public AddMasterDto Build()
        {
            return _dto;
        }
    }
}