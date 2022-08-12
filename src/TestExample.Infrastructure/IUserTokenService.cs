using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TestExample.Infrastructure
{
    public interface IUserTokenService
    {
        Guid UserId { get; }
    }

    public class UserTokenAppService : IUserTokenService
    {
        private readonly IHttpContextAccessor accessor;

        public UserTokenAppService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public Guid UserId => GetUserIdFromJwtToken();

        private Guid GetUserIdFromJwtToken()
        {
            var id = accessor.HttpContext.User.Claims
                .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier).Value;

            return Guid.Parse(id);
        }
    }
}