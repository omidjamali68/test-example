using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestExample.Entities.ApplicationIdentities;
using TestExample.Entities.CommonEntities;
using TestExample.Infrastructure;
using TestExample.Infrastructure.Application;
using TestExample.Infrastructure.Application.Validations;
using TestExample.Services.UserManagement.Contracts;
using TestExample.Services.UserManagement.Exceptions;

namespace TestExample.Services.UserManagement
{
    public class UserManagementAppService : IUserManagementService
    {
        private readonly IDateTimeService _dateTime;
        private readonly IApplicationUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementAppService(
            UserManager<ApplicationUser> userManager,
            IDateTimeService dateTime,
            IApplicationUserRepository repository,
            IUnitOfWork unitOfWork
        )
        {
            _userManager = userManager;
            _dateTime = dateTime;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateApplicationUserRequest(CreateApplicationUserRequestDto dto)
        {

            if (NationalCodeValidator.IsValid(dto.NationalCode) == false)
                throw new InvalidNationalCodeException();

            await GuardAgainstDuplicateRegisteration(dto.NationalCode);

            var userMobile = new Mobile
            {
                CountryCallingCode = "98",
                MobileNumber = dto.MobileNumber
            };

            var applicationUser = new ApplicationUser
            {
                NationalCode = dto.NationalCode,
                UserName = dto.NationalCode,
                Mobile = userMobile,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FatherName = dto.FatherName,
                Email = dto.Email,
                CreationDate = _dateTime.Now
            };

            var userCreationResult =
                await _userManager.CreateAsync(
                    applicationUser, applicationUser.Mobile.MobileNumber);
            if (!userCreationResult.Succeeded)
                throw new CreateApplicationUserFailedException();

            var addUserToRoleResult = await _userManager.AddToRoleAsync(
                applicationUser, dto.RoleName);
            if (!addUserToRoleResult.Succeeded)
                throw new AddUserToRoleFaildException();


            await _unitOfWork.CompleteAsync();

            return applicationUser.Id;
        }

        public async Task ChangePassword(ChangePasswordDto dto)
        {
            var applicationUser = await _userManager.GetUserAsync(dto.GetUserClaims());
            var identityResult =
                await _userManager.ChangePasswordAsync(
                    applicationUser, dto.CurrentPassword, dto.NewPassword);
            GuardAgainstChangePasswordFailed(identityResult);
        }

        public async Task<ApplicationUserDto> GetUserDtoById(Guid userId)
        {
            return await _repository.GetUserById(userId);
        }

        public async Task SetUserTimeZone(SetUserTimeZoneDto dto)
        {
            var applicationUser = await _repository.FindUserById(dto.GetUserId());
            GuardAgainstUserNotFound(applicationUser);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<ApplicationUser> FindByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public bool VerifyHashedPassword(ApplicationUser applicationUser, string dtoPassword)
        {
            var passwordVerifiedResult = _userManager.PasswordHasher.VerifyHashedPassword(
                applicationUser,
                applicationUser.PasswordHash,
                dtoPassword);

            if (passwordVerifiedResult == PasswordVerificationResult.Success)
                return true;
            return false;
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetClaimsAsync(applicationUser);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }

        private static void GuardAgainstChangePasswordFailed(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new ChangePasswordFailedException();
        }

        private static void GuardAgainstUserNotFound(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new UserNotFoundException();
        }

        private async Task GuardAgainstDuplicateRegisteration(string nationalCode)
        {
            if (await _repository.IsNationalCodeRegistered(nationalCode))
                throw new UserAlreadyExistException();
        }
    }
}