using Autofac;
using TestExample.Infrastructure;
using TestExample.Infrastructure.Application;
using TestExample.Infrastructure.Web;
using TestExample.Persistence.EF;
using TestExample.Persistence.EF.ApplicationIdentity;
using TestExample.Services.UserManagement;
using Microsoft.Extensions.Configuration;

namespace TestExample.RestApi.Configs
{
    internal class ServicesConfig : Configuration
    {
        private string _dbConnectionString;

        public override void Initialized()
        {
            _dbConnectionString = AppSettings.GetValue<string>("dbConnectionString");
        }

        public override void ConfigureServiceContainer(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(typeof(UserManagementAppService).Assembly)
                 .AssignableTo<IService>()
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            container.RegisterType<UtcDateTimeService>()
                .As<IDateTimeService>()
                .SingleInstance();

            container.RegisterType<UriSortParser>()
                .AsSelf()
                .InstancePerLifetimeScope();

            container.RegisterType<UriExpressionParser>()
                .AsSelf()
                .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            container.RegisterType<EFDataContext>()
                .WithParameter("connectionString", _dbConnectionString)
                .AsSelf()
                .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFApplicationUserRepository).Assembly)
                  .AssignableTo<IRepository>()
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}