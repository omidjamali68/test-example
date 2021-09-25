using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.Entities.CommonEntities;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Application.Validations;
using Cooking.Infrastructure.Sms.Contracts;
using Cooking.Services.UserManagement.Contracts;
using Cooking.Services.UserManagement.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Cooking.Services.UserManagement
{
    public class UserManagementAppService : UserManagementService
    {
        private const int MAX_VERIFICATION_CODE_SEND_PER_DAY = 6;
        private const int EXPIRE_VERIFICATIONCODE_TIME_MINUTE = 6;
        private readonly DateTimeService _dateTime;
        private readonly ApplicationUserRepository _repository;
        private readonly IBaseSmsService _smsService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementAppService(UserManager<ApplicationUser> userManager,
            DateTimeService dateTime,
            IBaseSmsService smsService,
            ApplicationUserRepository repository,
            UnitOfWork unitOfWork
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
            if (NationalCodeValidator.IsValid(dto.NationalCode) == false)
                throw new InvalidNationalCodeException();

            await GuardAgainstDuplicateRegisteration(dto.NationalCode);

            await GuardAgainstSendMoreSmsThanSpecifiedNumber(dto.NationalCode);

            //dto.CountryCallingCode = NormalizeCountryCallingCode(dto.CountryCallingCode);
            var userMobile = new Mobile
            {
                CountryCallingCode = "+98", //dto.CountryCallingCode,
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
                await _userManager.CreateAsync(applicationUser, applicationUser.Mobile.MobileNumber);
            if (!userCreationResult.Succeeded)
                throw new CreateApplicationUserFailedException();

            var addUserToRoleResult = await _userManager.AddToRoleAsync(applicationUser, dto.RoleName);
            if (!addUserToRoleResult.Succeeded)
                throw new AddUserToRoleFaildException();

            applicationUser = await _userManager.FindByNameAsync(dto.NationalCode);

            await _unitOfWork.Complete();

            //Send Verification Code
            //var verificationCode = VerificationCode.Generate();
            //var smsResult = SendSms(userMobile, SMSMessageTemplate.FepcoCookingVerificationCode,
            //    verificationCode);
            //await SaveApplicationUserVerificationCode(dto.NationalCode, userMobile, verificationCode, smsResult);
        }

        public async Task<Guid> ConfirmVerificationCodeAndCreateUser(ConfirmVerificationCodeDto dto)
        {
            await GuardAgainstDuplicateRegisteration(dto.NationalCode);

            var userVerification =
                await _repository.GetLastUserUnExpiredVerificationCode(dto.NationalCode,
                    EXPIRE_VERIFICATIONCODE_TIME_MINUTE);
            if (userVerification == null)
                throw new VerificationCodeNotExistException();
            if (userVerification.VerificationCode != dto.VerificationCode) throw new WrongVerificationCodeException();

            var applicationUser = new ApplicationUser();
            if (!await _repository.IsNationalCodeRegistered(dto.NationalCode))
            {
                applicationUser = new ApplicationUser
                {
                    NationalCode = userVerification.NationalCode,
                    UserName = userVerification.NationalCode,
                    Mobile = userVerification.Mobile,
                };

                var userCreationResult =
                    await _userManager.CreateAsync(applicationUser, applicationUser.Mobile.MobileNumber);
                if (!userCreationResult.Succeeded)
                    throw new CreateApplicationUserFailedException();

                var addUserToRoleResult = await _userManager.AddToRoleAsync(applicationUser, dto.roleName);
                if (!addUserToRoleResult.Succeeded)
                    throw new AddUserToRoleFaildException();

            }
            else
                applicationUser = await _userManager.FindByNameAsync(dto.NationalCode);

            await _repository.RemoveRegisterdUserVerificationCodes(applicationUser.NationalCode);
            await _unitOfWork.Complete();

            return applicationUser.Id;
        }

        //TODO:Resend not send
        public async Task SendVerificationCode(SendVerificationCodeDto dto)
        {
            await GuardAgainstDuplicateRegisteration(dto.NationalCode);

            await GuardAgainstSendMoreSmsThanSpecifiedNumber(dto.NationalCode);

            var userOldVerificationCode = await _repository.GetLastUserVerificationCode(dto.NationalCode);
            if (userOldVerificationCode == null) throw new NotRegisteredNationalCodeException();

            var verificationCode = VerificationCode.Generate();
            var userMobile = userOldVerificationCode.Mobile;

            var result = SendSms(userMobile,
            SMSMessageTemplate.FepcoCookingVerificationCode,
            verificationCode);

            await SaveApplicationUserVerificationCode(dto.NationalCode, userMobile, verificationCode, result);
        }

        public async Task ChangePassword(ChangePasswordDto dto)
        {
            var applicationUser = await _userManager.GetUserAsync(dto.GetUserClaims());
            var identityResult =
                await _userManager.ChangePasswordAsync(applicationUser, dto.CurrentPassword, dto.NewPassword);

            if (!identityResult.Succeeded)
                throw new ChangePasswordFailedException();
        }

        public async Task<AccountExistInfoDto> IsAccountExistAndVerified(AccountExistDto dto)
        {
            dto.CountryCallingCode = NormalizeCountryCallingCode(dto.CountryCallingCode);

            var accountInfo = new AccountExistInfoDto();
            var applicationUsers =
                await _repository.GetRegistredUsers(dto.NationalCode, dto.CountryCallingCode, dto.MobileNumber);

            //TODO: if not needed default bool value is false
            if (!applicationUsers.Any())
            {
                accountInfo.NationalCodeExist = false;
                accountInfo.MobileExist = false;
            }
            else
            {
                accountInfo.NationalCodeExist = applicationUsers.Any(_ => _.NationalCode == dto.NationalCode);
                accountInfo.MobileExist = applicationUsers.Any(_ => _.Mobile.MobileNumber == dto.MobileNumber
                                                                    && _.Mobile.CountryCallingCode ==
                                                                    dto.CountryCallingCode);
            }

            return accountInfo;
        }

        public async Task<ApplicationUserDto> GetUserDtoById(Guid userId)
        {
            return await _repository.GetUserById(userId);
        }

        public async Task AddUserToRole(Guid userId, string role)
        {
            var applicationUser = await _repository.FindUserById(userId);

            if (applicationUser == null)
                throw new UserNotFoundException();

            await _userManager.AddToRoleAsync(applicationUser, role);
        }

        public async Task<Guid> CreateVerifiedApplicationUser(CreateVerifiedApplicationUserDto dto)
        {
            dto.CountryCallingCode = NormalizeCountryCallingCode(dto.CountryCallingCode);

            if (await _repository.IsNationalCodeRegistered(dto.NationalCode))
            {
                var user = await _repository.FindUserByNationalCode(dto.NationalCode);
                if (!await _userManager.IsInRoleAsync(user, dto.roleName))
                    await _userManager.AddToRoleAsync(user, dto.roleName);

                return user.Id;
            }

            var applicationUser = new ApplicationUser
            {
                Id = dto.UserId,
                NationalCode = dto.NationalCode,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Mobile = new Mobile { CountryCallingCode = dto.CountryCallingCode, MobileNumber = dto.MobileNumber },
                UserName = dto.NationalCode,
                CreationDate = _dateTime.Now
            };

            var Identityresult = await _userManager.CreateAsync(applicationUser, dto.MobileNumber);

            if (!Identityresult.Succeeded)
                throw new CreateApplicationUserFailedException();

            await _userManager.AddToRoleAsync(applicationUser, dto.roleName);

            return applicationUser.Id;
        }

        public async Task SetUserTimeZone(SetUserTimeZoneDto dto)
        {
            var applicationUser = await _repository.FindUserById(dto.GetUserId());
            if (applicationUser == null)
                throw new UserNotFoundException();

            await _unitOfWork.Complete();
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

        public async Task<ApplicationUser> FindUserById(Guid userId)
        {
            return await _repository.FindUserById(userId);
        }

        //TODO:Move private method to end of class
        private async Task GuardAgainstDuplicateRegisteration(string nationalCode)
        {
            if (await _repository.IsNationalCodeRegistered(nationalCode))
                throw new UserAlreadyExistException();
        }

        //TODO:Move private method to end of class
        private async Task SaveApplicationUserVerificationCode(string userNationalCode, Mobile userMobile,
            uint verificationCode, string result)
        {
            _repository.AddUserVerificationCode(new IdentityVerificationCode
            {
                SMSResultDesc = result,
                VerificationCode = verificationCode,
                VerificationDate = _dateTime.Now,
                Mobile = new Mobile
                { CountryCallingCode = userMobile.CountryCallingCode, MobileNumber = userMobile.MobileNumber },
                NationalCode = userNationalCode
            });

            await _unitOfWork.Complete();
        }

        private string NormalizeCountryCallingCode(string countryCallingCode)
        {
            return countryCallingCode.TrimStart('0');
        }

        private async Task GuardAgainstSendMoreSmsThanSpecifiedNumber(string nationalCode)
        {
            if (await _repository.GetUsersSentSmesOfTodayCount(nationalCode) > MAX_VERIFICATION_CODE_SEND_PER_DAY)
                throw new MaxVerificationCodeCountException();
        }

        private string SendSms(Mobile mobile, SMSMessageTemplate smsMessageTemplate, uint activationCode)
        {
            var recipientNumbers = new List<Mobile> { mobile };

            var smsDto = ParameterizedSmsMessageDto.Create()
                .AddReceiver(recipientNumbers.Select(_ => _.CountryCallingCode + _.MobileNumber).ToArray())
                .WithTemplate(smsMessageTemplate)
                .SetParameter(SMSIRPatternParameters.TAAVSYSTEM_RECRUITMENT_VERIFICATION_CODE,
                    activationCode.ToString());

            var smsResultDesc = _smsService.SendParameterizedMessage(smsDto);
            return smsResultDesc;
        }
    }
}