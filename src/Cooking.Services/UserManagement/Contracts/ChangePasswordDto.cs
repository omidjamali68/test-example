using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Cooking.Services.UserManagement.Contracts
{
    public class ChangePasswordDto
    {
        private ClaimsPrincipal UserClaim;

        [Required] public string CurrentPassword { get; set; }

        [Required] public string NewPassword { get; set; }

        public void SetUserClaim(ClaimsPrincipal claim)
        {
            UserClaim = claim;
        }

        public ClaimsPrincipal GetUserClaims()
        {
            return UserClaim;
        }
    }
}