using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Entities.CommonEntities;
using Cooking.Infrastructure;
using Cooking.Services.UserManagement.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.ApplicationIdentity
{
    public class EFApplicationUserRepository : ApplicationUserRepository
    {
        private const int HOURS_OF_DAY = 24;
        private readonly DbSet<ApplicationUser> _applicationUsers;
        private readonly EFDataContext _context;
        private readonly DateTimeService _dateTime;

        public EFApplicationUserRepository(EFDataContext context, DateTimeService dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _applicationUsers = _context.Set<ApplicationUser>();
        }

        public void AddUserVerificationCode(IdentityVerificationCode verificationCode)
        {
            _context.VerificationCodes.Add(verificationCode);
        }

        public async Task<ApplicationUser> FindUserByNationalCode(string nationalCode)
        {
            //TODO:Remove where!!!
            return await _applicationUsers
                .LastOrDefaultAsync(_ => _.NationalCode == nationalCode);
        }

        //TODO:Remove User from Method Name
        public async Task<IdentityVerificationCode> GetLastUserUnExpiredVerificationCode(string nationalCode,
            int expireTime)
        {
            var now = _dateTime.Now.Subtract(new TimeSpan(0, expireTime, 0));

            return await _context.VerificationCodes
                .Where(_ => _.NationalCode == nationalCode && _.VerificationDate > now)
                .OrderByDescending(_ => _.VerificationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityVerificationCode> GetLastUserVerificationCode(string nationalCode)
        {
            return await _context.VerificationCodes
                .Where(_ => _.NationalCode == nationalCode)
                .OrderByDescending(_ => _.VerificationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUsersSentSmesOfTodayCount(string nationalCode)
        {
            var now = _dateTime.Now.Subtract(new TimeSpan(HOURS_OF_DAY, 0, 0));

            return await _context.VerificationCodes
                .Where(_ => _.NationalCode == nationalCode && _.VerificationDate > now)
                .CountAsync();
        }

        public async Task<Guid?> GetUserIdByNationalCode(string nationalCode)
        {
            return await _applicationUsers.Where(_ => _.NationalCode == nationalCode)
                .Select(_ => _.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> IsNationalCodeRegistered(string nationalCode)
        {
            return await _applicationUsers.AnyAsync(_ => _.NationalCode == nationalCode);
        }

        public async Task<ApplicationUserDto> GetUserById(Guid userId)
        {
            return await _applicationUsers.Where(_ => _.Id == userId)
                .Select(_ => new ApplicationUserDto
                {
                    Id = _.Id,
                    CountryCallingCode = _.Mobile.CountryCallingCode,
                    MobileNumber = _.Mobile.MobileNumber,
                    NationalCode = _.NationalCode,
                    UserName = _.UserName
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindUserById(Guid userId)
        {
            return await _applicationUsers.SingleOrDefaultAsync(_ => _.Id == userId);
        }

        public async Task<IList<ApplicationUser>> GetRegistredUsers(string nationalCode, string countryCallingCode,
            string mobileNumber)
        {
            return await _applicationUsers.Where(_ => _.Mobile.CountryCallingCode == countryCallingCode
                                                      && _.Mobile.MobileNumber == mobileNumber
                                                      || _.NationalCode == nationalCode)
                .Select(_ => new ApplicationUser
                {
                    NationalCode = _.NationalCode,
                    Mobile = new Mobile
                        {CountryCallingCode = _.Mobile.CountryCallingCode, MobileNumber = _.Mobile.MobileNumber}
                })
                .ToListAsync();
        }

        public async Task RemoveRegisterdUserVerificationCodes(string nationalCode)
        {
            var userVerificationCodes = await GetAllUserVerificationCodes(nationalCode);

            _context.VerificationCodes.RemoveRange(userVerificationCodes);
        }

        public async Task<IdentityVerificationCode> GetUserVerificationCode(string nationalCode)
        {
            return await _context.VerificationCodes.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode);
        }

        private async Task<List<IdentityVerificationCode>> GetAllUserVerificationCodes(string nationalCode)
        {
            return await _context.VerificationCodes.Where(_ => _.NationalCode == nationalCode).ToListAsync();
        }
    }
}