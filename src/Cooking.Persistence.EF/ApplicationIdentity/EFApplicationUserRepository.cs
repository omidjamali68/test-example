using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Infrastructure;
using Cooking.Services.UserManagement.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.ApplicationIdentity
{
    public class EFApplicationUserRepository : IApplicationUserRepository
    {
        private const int HOURS_OF_DAY = 24;
        private readonly DbSet<ApplicationUser> _applicationUsers;
        private readonly EFDataContext _context;
        private readonly IDateTimeService _dateTime;

        public EFApplicationUserRepository(EFDataContext context, IDateTimeService dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _applicationUsers = _context.Set<ApplicationUser>();
        }

        public void AddUserVerificationCode(IdentityVerificationCode verificationCode)
        {
            _context.VerificationCodes.Add(verificationCode);
        }

        //TODO:Remove User from Method Name
        public async Task<IdentityVerificationCode> GetLastUserUnExpiredVerificationCode(
            string phoneNumber,
            int expireTime)
        {
            var now = _dateTime.Now.Subtract(new TimeSpan(0, expireTime, 0));

            return await _context.VerificationCodes
                .Where(_ => _.PhoneNumber == phoneNumber && _.VerificationDate > now)
                .OrderByDescending(_ => _.VerificationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityVerificationCode> GetLastUserVerificationCode(
            string phoneNumber)
        {
            return await _context.VerificationCodes
                .Where(_ => _.PhoneNumber == phoneNumber)
                .OrderByDescending(_ => _.VerificationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUsersSentSmesOfTodayCount(string phoneNumber)
        {
            var now = _dateTime.Now.Subtract(new TimeSpan(HOURS_OF_DAY, 0, 0));

            return await _context.VerificationCodes
                .Where(_ => _.PhoneNumber == phoneNumber && _.VerificationDate > now)
                .CountAsync();
        }

        public async Task<bool> IsPhoneNumberRegistered(string phoneNumber)
        {
            return await _applicationUsers.AnyAsync(_ => _.PhoneNumber == phoneNumber);
        }

        public async Task<ApplicationUserDto> GetUserById(Guid userId)
        {
            return await _applicationUsers.Where(_ => _.Id == userId)
                .Select(_ => new ApplicationUserDto
                {
                    Id = _.Id,
                    MobileNumber = _.PhoneNumber,
                    NationalCode = _.NationalCode,
                    UserName = _.UserName
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindUserById(Guid userId)
        {
            return await _applicationUsers.SingleOrDefaultAsync(_ => _.Id == userId);
        }

        public async Task<IList<ApplicationUser>> GetRegistredUsers(
            string mobileNumber)
        {
            return await _applicationUsers.Where(_ => _.PhoneNumber == mobileNumber)
                .Select(_ => new ApplicationUser
                {
                    NationalCode = _.NationalCode,
                    PhoneNumber = _.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task RemoveRegisterdUserVerificationCodes(string phoneNumber)
        {
            _context.VerificationCodes.RemoveRange(
                await GetAllUserVerificationCodes(phoneNumber));
        }

        private async Task<List<IdentityVerificationCode>> GetAllUserVerificationCodes(string phoneNumber)
        {
            return await _context.VerificationCodes.Where(_ => _.PhoneNumber == phoneNumber)
                .ToListAsync();
        }
    }
}