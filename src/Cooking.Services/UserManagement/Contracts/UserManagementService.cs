using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.UserManagement.Contracts
{
    public interface UserManagementService : Service
    {
        Task CreateApplicationUserRequest(CreateApplicationUserRequestDto dto);
        Task<Guid> ConfirmVerificationCodeAndCreateUser(ConfirmVerificationCodeDto confirmVerificationCodeDto);
        Task SendVerificationCode(SendVerificationCodeDto dto);
        Task ChangePassword(ChangePasswordDto dto);
        Task<AccountExistInfoDto> IsAccountExistAndVerified(AccountExistDto dto);
        Task<ApplicationUserDto> GetUserDtoById(Guid userId);
        Task<ApplicationUser> FindUserById(Guid userId);
        Task AddUserToRole(Guid userId, string role);
        Task<Guid> CreateVerifiedApplicationUser(CreateVerifiedApplicationUserDto dto);
        Task SetUserTimeZone(SetUserTimeZoneDto dto);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser);
        Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser);
        Task<ApplicationUser> FindByUsername(string username);
        bool VerifyHashedPassword(ApplicationUser applicationUser, string dtoPassword);
    }
}