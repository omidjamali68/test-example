using System;
using Microsoft.Extensions.Logging;
using Moq;
using TestExample.Persistence.EF;
using TestExample.Persistence.EF.Masters;
using TestExample.Services.Masters;
using TestExample.Services.UserManagement.Contracts;

namespace TestExample.TestTools.Masters
{
    public static class MasterFactory
    {
        public static MasterAppService CreateService(EFDataContext context)
        {
            var repository = new EFMasterRepository(context);
            var unitOfWork = new EFUnitOfWork(context);
            var userManagementService = new Mock<IUserManagementService>();
            userManagementService.Setup(_ => _.CreateApplicationUserRequest(
                It.IsAny<CreateApplicationUserRequestDto>()))
                .ReturnsAsync(Guid.NewGuid());
            var logger = new Mock<ILogger>();
            return new MasterAppService(
                repository, unitOfWork, userManagementService.Object, logger.Object);
        }
    }
}