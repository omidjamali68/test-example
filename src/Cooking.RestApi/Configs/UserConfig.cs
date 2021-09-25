using System;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cooking.RestApi.Configs
{
    internal class UserConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddHttpContextAccessor();
        }

        public override void ConfigureServiceContainer(ContainerBuilder container)
        {
            container.RegisterType<HttpContextUserIdentity>().As<UserIdentity>();
        }
    }

    public interface UserIdentity
    {
        string Id { get; }
        bool IsAuthenticated { get; }
    }

    internal class HttpContextUserIdentity : UserIdentity
    {
        public HttpContextUserIdentity(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var subUserClaim = httpContext.User.Claims.FirstOrDefault(_ =>
                _.Properties.Any(_ => _.Value.Equals("sub", StringComparison.OrdinalIgnoreCase)));
            Id = subUserClaim?.Value;
        }

        public string Id { get; }

        public bool IsAuthenticated => Id != null;
    }
}