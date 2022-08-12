using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.UserManagement.Contracts
{
    public interface IApplicationUserRepository : IRepository
    {
        Task<bool> IsNationalCodeRegistered(string nationalCode);
        Task<ApplicationUserDto> GetUserById(Guid userId);
        Task<ApplicationUser> FindUserById(Guid userId);
        Task<IList<ApplicationUser>> GetRegistredUsers(string nationalCode);
    }
}