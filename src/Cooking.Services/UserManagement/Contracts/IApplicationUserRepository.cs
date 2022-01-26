using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.UserManagement.Contracts
{
    public interface IApplicationUserRepository : IRepository
    {
        void AddUserVerificationCode(IdentityVerificationCode verificationCode);
        Task<IdentityVerificationCode> GetLastUserUnExpiredVerificationCode(string phoneNumber, int expireTime);
        Task<IdentityVerificationCode> GetLastUserVerificationCode(string phoneNumber);
        Task<int> GetUsersSentSmesOfTodayCount(string phoneNumber);
        Task<bool> IsPhoneNumberRegistered(string phoneNumber);
        Task<ApplicationUserDto> GetUserById(Guid userId);
        Task<ApplicationUser> FindUserById(Guid userId);
        Task<IList<ApplicationUser>> GetRegistredUsers(string phoneNumber);
        Task RemoveRegisterdUserVerificationCodes(string phoneNumber);
    }
}