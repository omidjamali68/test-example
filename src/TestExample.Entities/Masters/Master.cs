using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Entities.Universities;

namespace TestExample.Entities.Masters
{
    public class Master
    {
        public Master(
            string firstName,
            string lastName,
            string nationalCode,
            string mobile,
            int universityId,
            Guid userId)
        {
            FirstName = firstName;
            LastName = lastName;
            NationalCode = nationalCode;
            Mobile = mobile;
            UniversityId = universityId;
            UserId = userId;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public int UniversityId { get; set; }
        public University University { get; set; }
        public Guid UserId { get; set; }
    }
}