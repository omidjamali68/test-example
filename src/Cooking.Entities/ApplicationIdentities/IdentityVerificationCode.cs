using System;
using Cooking.Entities.CommonEntities;

namespace Cooking.Entities.ApplicationIdentities
{
    public class IdentityVerificationCode
    {
        public long Id { get; set; }
        public uint VerificationCode { get; set; }
        public DateTime VerificationDate { get; set; }
        public string SMSResultDesc { get; set; }
        public string NationalCode { get; set; }
        public Mobile Mobile { get; set; }
    }
}