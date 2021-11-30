using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.Services.RecipeServices.StepOperations.Exceptions;
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
            await GuardAgainstStepOperationTitleExist(dto.Title);
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

        public async Task Delete(long id)
        {
            var stepOperation = await _stepOperationRepository.FindById(id);
            GuardAgainstStepOperationNotFound(stepOperation);

            await GuardAgainstStepOperationUsedInRecipe(stepOperation);

            _stepOperationRepository.Remove(stepOperation);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateStepOperationDto dto, long id)
        {
            var stepOperation = await _stepOperationRepository.FindById(id);
            GuardAgainstStepOperationNotFound(stepOperation);
            await GuardAgainstStepOperationTitleExist(dto.Title, id);

            stepOperation.AvatarId = dto.AvatarId;
            stepOperation.Extension = dto.Extension;
            stepOperation.Title = dto.Title;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetStepOperationDto> GetStepOperation(long id)
        {
            var stepOperation = await _stepOperationRepository.FindById(id);
            GuardAgainstStepOperationNotFound(stepOperation);

            return new GetStepOperationDto
            {
                AvatarId = stepOperation.AvatarId,
                Extension = stepOperation.Extension,
                Title = stepOperation.Title
            };
        }

        #region Guard Methods

        private async Task GuardAgainstStepOperationUsedInRecipe(StepOperation stepOperation)
        {
            var isUsedInRecipe = await _stepOperationRepository.ExistInRecipe(stepOperation.Id);
            if (isUsedInRecipe)
                throw new StepOperationUseInRecipeException();
        }

        private void GuardAgainstStepOperationNotFound(StepOperation stepOperation)
        {
            if (stepOperation == null)
                throw new StepOperationNotFoundException();
        }

        private async Task GuardAgainstStepOperationTitleExist(string title, long? id = null)
        {
            var isTitleExist = await _stepOperationRepository.IsTitleExist(title, id);
            if (isTitleExist)
                throw new StepOperationTitleExistException();
        }

        public async Task<PageResult<GetAllStepOperationDto>> GetAllStepOperation(
            string searchText,
            Pagination? pagination,
            Sort<GetAllStepOperationDto>? sortExpression)
        {
            return await _stepOperationRepository.GetAll(searchText, pagination, sortExpression);
        }

        #endregion
    }
}
