using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.UserManagement.Contracts
{
    public interface ApplicationUserRepository : Repository
    {
        void AddUserVerificationCode(IdentityVerificationCode verificationCode);
        Task<Guid?> GetUserIdByNationalCode(string nationalCode);
        Task<ApplicationUser> FindUserByNationalCode(string nationalCode);
        Task<IdentityVerificationCode> GetLastUserUnExpiredVerificationCode(string nationalCode, int expireTime);
        Task<IdentityVerificationCode> GetLastUserVerificationCode(string nationalCode);
        Task<int> GetUsersSentSmesOfTodayCount(string nationalCode);
        Task<bool> IsNationalCodeRegistered(string nationalCode);
        Task<ApplicationUserDto> GetUserById(Guid userId);
        Task<ApplicationUser> FindUserById(Guid userId);

        Task<IList<ApplicationUser>> GetRegistredUsers(string nationalCode, string countryCallingCode,
            string mobileNumber);

        Task RemoveRegisterdUserVerificationCodes(string nationalCode);
        Task<IdentityVerificationCode> GetUserVerificationCode(string nationalCode);
    }
}