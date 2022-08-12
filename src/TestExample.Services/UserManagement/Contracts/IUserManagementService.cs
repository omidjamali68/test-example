using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.UserManagement.Contracts
{
    public interface IUserManagementService : IService
    {
        Task<Guid> CreateApplicationUserRequest(CreateApplicationUserRequestDto dto);
        Task ChangePassword(ChangePasswordDto dto);
        Task<ApplicationUserDto> GetUserDtoById(Guid userId);
        Task SetUserTimeZone(SetUserTimeZoneDto dto);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser);
        Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser);
        Task<ApplicationUser> FindByUsername(string username);
        bool VerifyHashedPassword(ApplicationUser applicationUser, string dtoPassword);
    }
}