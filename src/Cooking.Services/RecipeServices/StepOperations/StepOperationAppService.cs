using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.StepOperations
{
    public class StepOperationAppService : IStepOperationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStepOperationRepository _stepOperationRepository;

        public StepOperationAppService(IUnitOfWork unitOfWork,
            IStepOperationRepository stepOperationRepository)
        {
            _unitOfWork = unitOfWork;
            _stepOperationRepository = stepOperationRepository;
        }

        public async Task<long> AddAsync(AddStepOperationDto dto)
        {
            StepOperation stepOperation = new StepOperation
            {
                AvatarId = dto.AvatarId,
                Extension = dto.Extension,
                Title = dto.Title
            };

            await _stepOperationRepository.AddAsync(stepOperation);
            await _unitOfWork.CompleteAsync();
            return stepOperation.Id;
        }
    }
}
