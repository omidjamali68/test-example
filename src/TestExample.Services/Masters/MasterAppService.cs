using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestExample.Entities.Masters;
using TestExample.Infrastructure;
using TestExample.Infrastructure.Application;
using TestExample.Services.Masters.Contracts;
using TestExample.Services.UserManagement.Contracts;

namespace TestExample.Services.Masters
{
    public class MasterAppService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger _logger;

        public MasterAppService(
            IMasterRepository masterRepository,
            IUnitOfWork unitOfWork,
            IUserManagementService userManagementService,
            ILogger logger)
        {
            _masterRepository = masterRepository;
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
            _logger = logger;
        }

        public async Task<int> Add(AddMasterDto dto)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                Guid userId = await CreateUser(dto);

                var master = new Master(
                    dto.FirstName,
                    dto.LastName,
                    dto.NationalCode,
                    dto.Mobile,
                    dto.UniversityId,
                    userId);
                _masterRepository.Add(master);

                await _unitOfWork.CommitAsync();
                return master.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError("Error in add master with user: " + ex);
                throw;
            }

        }

        private async Task<Guid> CreateUser(AddMasterDto dto)
        {
            var userDto = new CreateApplicationUserRequestDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MobileNumber = dto.Mobile,
                NationalCode = dto.NationalCode,
                RoleName = UserRoles.Master
            };
            var userId =
                await _userManagementService.CreateApplicationUserRequest(userDto);
            _unitOfWork.CommitPartial();
            return userId;
        }
    }
}