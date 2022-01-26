using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Application.Validations;
using Cooking.Infrastructure.Sms.Contracts;
using Cooking.Services.UserManagement.Contracts;
using Cooking.Services.UserManagement.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Cooking.Services.UserManagement
{
    public class UserManagementAppService : IUserManagementService
    {
        private const int MAX_VERIFICATION_CODE_SEND_PER_DAY = 6;
        private const int EXPIRE_VERIFICATIONCODE_TIME_MINUTE = 6;
        private readonly IDateTimeService _dateTime;
        private readonly IApplicationUserRepository _repository;
        private readonly IBaseSmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementAppService(UserManager<ApplicationUser> userManager,
            IDateTimeService dateTime,
            IBaseSmsService smsService,
            IApplicationUserRepository repository,
            IUnitOfWork unitOfWork
        )
        {
            _userManager = userManager;
            _dateTime = dateTime;
            _smsService = smsService;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateApplicationUserRequest(CreateApplicationUserRequestDto dto)
        {

            await GuardAgainstDuplicateRegisteration(dto.PhoneNumber);

            await GuardAgainstSendMoreSmsThanSpecifiedNumber(dto.PhoneNumber);

            var applicationUser = new ApplicationUser
            {
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                CreationDate = _dateTime.Now
            };

            var userCreationResult =
                await _userManager.CreateAsync(applicationUser, applicationUser.PhoneNumber);
            GuardAgainstCreateApplicationUserFailed(userCreationResult);

            var addUserToRoleResult = await _userManager.AddToRoleAsync(applicationUser, dto.RoleName);
            GuardAgainstAddUserToRoleFaild(addUserToRoleResult);

            applicationUser = await _userManager.FindByNameAsync(dto.PhoneNumber);

            await _unitOfWork.CompleteAsync();

            var verificationCode = VerificationCode.Generate();
            var smsResult = SendSms(dto.PhoneNumber, SMSMessageTemplate.CookingVerificationCode,
               verificationCode);
            await SaveApplicationUserVerificationCode(dto.PhoneNumber, verificationCode, smsResult);
        }

        public async Task<Guid> ConfirmVerificationCodeAndCreateUser(ConfirmVerificationCodeDto dto)
        {
            await GuardAgainstDuplicateRegisteration(dto.PhoneNumber);

            var userVerification =
                await _repository.GetLastUserUnExpiredVerificationCode(dto.PhoneNumber,
                    EXPIRE_VERIFICATIONCODE_TIME_MINUTE);
            GuardAgainstVerificationCodeNotExist(userVerification);
            if (userVerification.VerificationCode != dto.VerificationCode) throw new WrongVerificationCodeException();

            var applicationUser = new ApplicationUser();
            if (!await _repository.IsPhoneNumberRegistered(dto.PhoneNumber))
            {
                applicationUser = new ApplicationUser
                {
                    UserName = userVerification.PhoneNumber,
                    PhoneNumber = userVerification.PhoneNumber,
                };

                var userCreationResult =
                    await _userManager.CreateAsync(applicationUser, applicationUser.PhoneNumber);
                GuardAgainstCreateApplicationUserFailed(userCreationResult);

                var addUserToRoleResult = await _userManager.AddToRoleAsync(applicationUser, dto.roleName);
                GuardAgainstAddUserToRoleFaild(addUserToRoleResult);

            }
            else
                applicationUser = await _userManager.FindByNameAsync(dto.PhoneNumber);

            await _repository.RemoveRegisterdUserVerificationCodes(applicationUser.PhoneNumber);
            await _unitOfWork.CompleteAsync();

            return applicationUser.Id;
        }

        public async Task SendVerificationCode(SendVerificationCodeDto dto)
        {
            await GuardAgainstDuplicateRegisteration(dto.PhoneNumber);

            await GuardAgainstSendMoreSmsThanSpecifiedNumber(dto.PhoneNumber);

            var userOldVerificationCode = await _repository.GetLastUserVerificationCode(dto.PhoneNumber);
            GuardAgainstNotRegister(userOldVerificationCode);

            var verificationCode = VerificationCode.Generate();

            var result = SendSms(
                userOldVerificationCode.PhoneNumber,
                SMSMessageTemplate.CookingVerificationCode,
                verificationCode);

            await SaveApplicationUserVerificationCode(dto.PhoneNumber, verificationCode, result);
        }

        public async Task ChangePassword(ChangePasswordDto dto)
        {
            var applicationUser = await _userManager.GetUserAsync(dto.GetUserClaims());
            var identityResult =
                await _userManager.ChangePasswordAsync(applicationUser, dto.CurrentPassword, dto.NewPassword);
            GuardAgainstChangePasswordFailed(identityResult);
        }

        public async Task<AccountExistInfoDto> IsAccountExistAndVerified(AccountExistDto dto)
        {
            var accountInfo = new AccountExistInfoDto();
            var applicationUsers =
                await _repository.GetRegistredUsers(dto.MobileNumber);

            if (!applicationUsers.Any())
            {
                accountInfo.MobileExist = false;
            }
            else
            {
                accountInfo.MobileExist = applicationUsers.Any(_ => _.PhoneNumber == dto.MobileNumber);
            }

            return accountInfo;
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
            var passwordVerifiedResult = _userManager.PasswordHasher.VerifyHashedPassword(applicationUser,
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

        private static void GuardAgainstVerificationCodeNotExist(IdentityVerificationCode userVerification)
        {
            if (userVerification == null)
                throw new VerificationCodeNotExistException();
        }

        private static void GuardAgainstNotRegister(IdentityVerificationCode userOldVerificationCode)
        {
            if (userOldVerificationCode == null) throw new NotRegisteredUserException();
        }
        
        private static void GuardAgainstAddUserToRoleFaild(IdentityResult addUserToRoleResult)
        {
            if (!addUserToRoleResult.Succeeded)
                throw new AddUserToRoleFaildException();
        }

        private static void GuardAgainstCreateApplicationUserFailed(IdentityResult userCreationResult)
        {
            if (!userCreationResult.Succeeded)
                throw new CreateApplicationUserFailedException();
        }

        private static void GuardAgainstInvalidNationalCode(string nationalCode)
        {
            if (NationalCodeValidator.IsValid(nationalCode) == false)
                throw new InvalidNationalCodeException();
        }
        
        private async Task GuardAgainstDuplicateRegisteration(string phoneNumber)
        {
            if (await _repository.IsPhoneNumberRegistered(phoneNumber))
                throw new UserAlreadyExistException();
        }

        private async Task SaveApplicationUserVerificationCode(string phoneNumber,
            uint verificationCode, string result)
        {
            _repository.AddUserVerificationCode(new IdentityVerificationCode
            {
                SMSResultDesc = result,
                VerificationCode = verificationCode,
                VerificationDate = _dateTime.Now,
                PhoneNumber = phoneNumber,
            });

            await _unitOfWork.CompleteAsync();
        }

        private async Task GuardAgainstSendMoreSmsThanSpecifiedNumber(string phoneNumber)
        {
            if (await _repository.GetUsersSentSmesOfTodayCount(phoneNumber) > MAX_VERIFICATION_CODE_SEND_PER_DAY)
                throw new MaxVerificationCodeCountException();
        }

        private string SendSms(string mobile, SMSMessageTemplate smsMessageTemplate, uint activationCode)
        {
            var recipientNumbers = new List<string> { mobile };

            var smsDto = ParameterizedSmsMessageDto.Create()
                .AddReceiver(recipientNumbers.ToArray())
                .WithTemplate(smsMessageTemplate)
                .SetParameter(SMSIRPatternParameters.TAAVSYSTEM_RECRUITMENT_VERIFICATION_CODE,
                    activationCode.ToString());

            var smsResultDesc = _smsService.SendParameterizedMessage(smsDto);
            return smsResultDesc;
        }
    }
}