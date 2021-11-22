using Cooking.Entities.Documents;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.RecipePersistence.StepOperations;
using Cooking.Services.RecipeServices.StepOperations;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Cooking.TestTools.DocumentTestTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.TestTools.RecipeTestTools.StepOperations
{
    public static class StepOperationFactory
    {
        public static StepOperationAppService CreateService(EFDataContext context)
        {
            var unitOfWork = new EFUnitOfWork(context);
            var repository = new EFStepOperationRepository(context);
            return new StepOperationAppService(unitOfWork, repository);
        }

        public static AddStepOperationDto GenerateAddDto(string title, Guid avatarId)
        {
            return new AddStepOperationDto
            {
                Title = title,
                AvatarId = avatarId,
                Extension = "jpg"
            };
        }

        public static UpdateStepOperationDto GenerateUpdateDto(Document document, string title = "سرخ کردن")
        {
            return new UpdateStepOperationDto
            {
                Title = title,
                AvatarId = document.Id,
                Extension = document.Extension
            };
        }
    }
}
