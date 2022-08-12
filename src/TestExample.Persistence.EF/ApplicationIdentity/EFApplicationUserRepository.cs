using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Services.UserManagement.Contracts;

namespace TestExample.Persistence.EF.ApplicationIdentity
{
    public class EFApplicationUserRepository : IApplicationUserRepository
    {
        private readonly DbSet<ApplicationUser> _applicationUsers;
        private readonly EFDataContext _context;

        public EFApplicationUserRepository(EFDataContext context)
        {
            _context = context;
            _applicationUsers = _context.Set<ApplicationUser>();
        }

        public async Task<bool> IsNationalCodeRegistered(string nationalCode)
        {
            return await _applicationUsers.AnyAsync(_ =>
                _.NationalCode == nationalCode);
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
            string nationalCode)
        {
            return await _applicationUsers.Where(_ => _.NationalCode == nationalCode)
                .Select(_ => new ApplicationUser
                {
                    NationalCode = _.NationalCode,
                    PhoneNumber = _.PhoneNumber
                })
                .ToListAsync();
        }
    }
}