using Autofac;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Sms;
using Cooking.Infrastructure.Sms.Contracts;
using Cooking.Infrastructure.Web;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.ApplicationIdentity;
using Cooking.Services.UserManagement;
using Microsoft.Extensions.Configuration;

namespace Cooking.RestApi.Configs
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

            container.RegisterType<SMSIRMessageService>()
                .As<IBaseSmsService>()
                .SingleInstance();

            container.RegisterType<MagickImagingService>()
                .As<IImagingService>()
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