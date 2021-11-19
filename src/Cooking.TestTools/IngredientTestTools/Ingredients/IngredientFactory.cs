using Cooking.Persistence.EF;
using Cooking.Persistence.EF.Documents;
using Cooking.Persistence.EF.IngredientPersistence.Ingredients;
using Cooking.Persistence.EF.IngredientPersistence.IngredientUnits;
using Cooking.Services.IngredientServices.Ingredients;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using System;

namespace Cooking.TestTools.IngredientTestTools.Ingredients
{
    public static class IngredientFactory
    {
        public static IngredientAppService CreateService(EFDataContext context)
        {
            var repository = new EFIngredientRepository(context);
            var ingredientUnitRepository = new EFIngredientUnitRepository(context);
            var documentRepository = new EFDocumentRepository(context);
            var unitOfWork = new EFUnitOfWork(context);

            return new IngredientAppService(
                repository,
                ingredientUnitRepository,
                documentRepository,
                unitOfWork);
        }

        public static AddIngredientDto GenerateAddIngredientDto(
            int ingredientUnitId,
            string title,
            Guid avatarId)
        {
            return new AddIngredientDto
            {
                IngredientUnitId = ingredientUnitId,
                Title = title,
                AvatarId = avatarId,
                Extension = "jpg"
            };
        } 
    }
}
