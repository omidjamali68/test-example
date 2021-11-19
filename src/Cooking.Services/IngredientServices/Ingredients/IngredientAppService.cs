using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using Cooking.Services.Documents.Contracts;
using Cooking.Services.Documents.Exceptions;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Cooking.Services.Ingredients.IngredientUnits.Contracts;
using Cooking.Services.Ingredients.IngredientUnits.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.IngredientServices.Ingredients
{
    public class IngredientAppService : IIngredientService
    {
        private readonly IIngredientRepository _repository;
        private readonly IIngredientUnitRepository _ingredientUnitRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientAppService(
            IIngredientRepository repository,
            IIngredientUnitRepository ingredientUnitRepository,
            IDocumentRepository documentRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _ingredientUnitRepository = ingredientUnitRepository;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddAsync(AddIngredientDto dto)
        {
            var ingredientUnit = await _ingredientUnitRepository.FindByIdAsync(dto.IngredientUnitId);
            GuardAgainstIngredientUnitNotFound(ingredientUnit);

            var ingredient = new Ingredient
            {
                Title = dto.Title,
                AvatarId = dto.AvatarId,
                Extension = dto.Extension,
                IngredientUnitId = ingredientUnit.Id,
            };

            await InsertDocumentAsync(dto.AvatarId);

            await _repository.AddAsync(ingredient);
            await _unitOfWork.CompleteAsync();
            return ingredient.Id;
        }


        #region Helper Methods
        private async Task InsertDocumentAsync(Guid avatarId)
        {
            await GuardAgainstDocumentNotExist(avatarId);
            await _documentRepository.RegisterDocument(avatarId);
        }
        #endregion

        #region Guard Methods
        private void GuardAgainstIngredientUnitNotFound(IngredientUnit ingredientUnit)
        {
           _ = ingredientUnit ?? throw new IngredientUnitNotFoundException();
        }

        private async Task GuardAgainstDocumentNotExist(Guid avatarId)
        {
            _ = await _documentRepository.FindById(avatarId) ?? throw new DocumentNotFoundException();
        }
        #endregion
    }
}
