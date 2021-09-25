using Autofac;
using Cooking.Infrastructure;
using Cooking.Infrastructure.Application;
using Cooking.Infrastructure.Sms;
using Cooking.Infrastructure.Sms.Contracts;
using Cooking.Infrastructure.Web;
using Cooking.Persistence.EF;
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
            //container.RegisterAssemblyTypes(typeof(RoutinePaymentAppService).Assembly)
            //    .AssignableTo<Service>()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();

            container.RegisterType<UtcDateTimeService>()
                .As<DateTimeService>()
                .SingleInstance();

            container.RegisterType<SMSIRMessageService>()
                .As<IBaseSmsService>()
                .SingleInstance();

            container.RegisterType<MagickImagingService>()
                .As<ImagingService>()
                .SingleInstance();

            container.RegisterType<UriSortParser>()
                .AsSelf()
                .InstancePerLifetimeScope();

            container.RegisterType<UriExpressionParser>()
                .AsSelf()
                .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                .As<UnitOfWork>()
                .InstancePerLifetimeScope();

            container.RegisterType<EFDataContext>()
                .WithParameter("connectionString", _dbConnectionString)
                .AsSelf()
                .InstancePerLifetimeScope();

            //container.RegisterAssemblyTypes(typeof(EFRoutinePaymentRepository).Assembly)
            //    .AssignableTo<Repository>()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
        }
    }
}