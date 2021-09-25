using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cooking.Entities.ApplicationIdentities;
using Cooking.RestApi.Configs;
using Cooking.Services.UserManagement.Contracts;
using Cooking.Services.UserManagement.Exceptions;
using Cooking.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cooking.RestApi.Controllers
{
    [Route("api/user-management")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementService _service;
        private readonly UserIdentity _userService;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;


        public UserManagementController(UserManagementService service,
            UserIdentity userService,
            IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _service = service;
            _userService = userService;
            jwtBearerTokenSettings = jwtTokenOptions.Value;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register-user")]
        public async Task Register(CreateApplicationUserRequestDto dto)
        {
            await _service.CreateApplicationUserRequest(dto);
        }

        //[HttpPut("confirm-verification-code")]
        //public async Task<Guid> ConfirmVerificationCode(ConfirmVerificationCodeDto dto)
        //{
        //    return await _service.ConfirmVerificationCodeAndCreateUser(dto);
        //}

        //[HttpPost("send-verification-code")]
        //public async Task SendVerificationCode(SendVerificationCodeDto dto)
        //{
        //    await _service.SendVerificationCode(dto);
        //}

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("change-password")]
        public async Task ChangePassword(ChangePasswordDto dto)
        {
            dto.SetUserClaim(HttpContext.User);

            await _service.ChangePassword(dto);
        }

        [Obsolete]
        [HttpGet("account-exist")]
        public async Task<AccountExistInfoDto> AccountExist(
            [FromQuery] [Required] string nationalCode,
            [FromQuery] [Required] string mobileNumber,
            [FromQuery] [Required] string countryCallingCode)
        {
            var dto = new AccountExistDto
            {
                CountryCallingCode = countryCallingCode,
                MobileNumber = mobileNumber,
                NationalCode = nationalCode
            };
            return await _service.IsAccountExistAndVerified(dto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("info")]
        public async Task<ApplicationUserDto> GetUserById()
        {
            var userId = Guid.Parse(_userService.Id);

            return await _service.GetUserDtoById(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(ApplicationUserLoginDto dto)
        {
            var applicationUser = await ValidateApplicationUser(dto);

            if (applicationUser == null)
                throw new WrongUsernameOrPasswordException();

            if (applicationUser.LockoutEnabled)
                throw new UserIsInactiveException();

            return Ok(await GenerateToken(applicationUser));
        }

        private async Task<ApplicationUser> ValidateApplicationUser(ApplicationUserLoginDto dto)
        {
            var applicationUser = await _service.FindByUsername(dto.Username);

            if (applicationUser == null)
                throw new UserNotFoundException();

            var passwordVerified = _service.VerifyHashedPassword(applicationUser, dto.Password);

            if (passwordVerified)
                return applicationUser;
            return null;
        }

        private async Task<string> GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var userClaims = await _service.GetClaimsAsync(applicationUser);
            var userRoles = await _service.GetRolesAsync(applicationUser);

            var tokenClaims = new ClaimsIdentity();
            tokenClaims.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString("N")));

            WriteUserRolesToTokenClaims(ref tokenClaims, userRoles);
            WriteUserClaimsToTokenClaims(ref tokenClaims, userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = tokenClaims,

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void WriteUserClaimsToTokenClaims(ref ClaimsIdentity tokenClaims, IList<Claim> userClaims)
        {
            foreach (var claim in userClaims) tokenClaims.AddClaim(new Claim(claim.Type, claim.Value));
        }

        private void WriteUserRolesToTokenClaims(ref ClaimsIdentity tokenClaims, IList<string> userRoles)
        {
            foreach (var role in userRoles) tokenClaims.AddClaim(new Claim(ClaimTypes.Role, role));
        }
    }
}