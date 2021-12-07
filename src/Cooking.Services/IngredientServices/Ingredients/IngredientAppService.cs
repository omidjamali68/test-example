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
using Cooking.Services.IngredientServices.Ingredients.Exceptions;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;

namespace Cooking.Services.IngredientServices.Ingredients
{
    public class IngredientAppService : IIngredientService
    {
        private readonly IIngredientRepository _repository;
        private readonly IIngredientUnitRepository _ingredientUnitRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientAppService(
            IIngredientRepository repository,
            IIngredientUnitRepository ingredientUnitRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IDocumentRepository documentRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _ingredientUnitRepository = ingredientUnitRepository;
            _recipeIngredientRepository = recipeIngredientRepository;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> AddAsync(AddIngredientDto dto)
        {
            var ingredientUnit = await _ingredientUnitRepository.FindByIdAsync(dto.IngredientUnitId);
            GuardAgainstIngredientUnitNotFound(ingredientUnit);

            await GuardAgainstIngredientTitleAndUnitExist(dto.Title, dto.IngredientUnitId);

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

        public async Task UpdateAsync(long id, UpdateIngredientDto dto)
        {
            var ingredient = await _repository.FindByIdAsync(id);
            GuardAgainstIngredientNotFound(ingredient);
            await GuardAgainstIngredientTitleAndUnitExist(dto.Title, dto.IngredientUnitId, id);

            var ingredientUnit = await _ingredientUnitRepository.FindByIdAsync(dto.IngredientUnitId);
            GuardAgainstIngredientUnitNotFound(ingredientUnit);

            ingredient.Title = dto.Title;
            ingredient.IngredientUnitId = dto.IngredientUnitId;

            if (ingredient.AvatarId != dto.AvatarId)
            {
                await UpdateDocumentAsync(
                    avatarId: dto.AvatarId,
                    oldAvatarId: ingredient.AvatarId);
                ingredient.AvatarId = dto.AvatarId;
                ingredient.Extension = dto.Extension;
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var ingredient = await _repository.FindByIdAsync(id);
            GuardAgainstIngredientNotFound(ingredient);

            await GuardAgainstIngredientUsedInRecipe(ingredient.Id);

            _repository.Remove(ingredient);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetIngredientDto> GetAsync(long id)
        {
            var ingredient = await _repository.FindByIdAsync(id);
            GuardAgainstIngredientNotFound(ingredient);

            return new GetIngredientDto
            {
                IngredientUnitId = ingredient.IngredientUnitId,
                Title = ingredient.Title,
                AvatarId = ingredient.AvatarId,
                Extension = ingredient.Extension
            };
        }

        public async Task<PageResult<GetAllIngredientDto>> GetAllAsync(
            string searchText,
            Pagination pagination,
            Sort<GetAllIngredientDto> sortExpression)
        {
            return await _repository.GetAllAsync(searchText, pagination, sortExpression);
        }


        #region Helper Methods
        private async Task InsertDocumentAsync(Guid avatarId)
        {
            await GuardAgainstDocumentNotExist(avatarId);
            await _documentRepository.RegisterDocument(avatarId);
        }

        private async Task UpdateDocumentAsync(Guid avatarId, Guid oldAvatarId)
        {
            await GuardAgainstDocumentNotExist(avatarId);
            await _documentRepository.DeleteByIds(new List<Guid> { oldAvatarId });
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

        private void GuardAgainstIngredientNotFound(Ingredient ingredient)
        {
            _ = ingredient ?? throw new IngredientNotFoundException();
        }

        private async Task GuardAgainstIngredientTitleAndUnitExist(
            string title,
            int ingredientUnitId,
            long? id = null)
        {
            if (await _repository.IsTitleAndUnitExistAsync(title, ingredientUnitId, id))
                throw new IngredientTitleAndUnitExistException();
        }

        private async Task GuardAgainstIngredientUsedInRecipe(long id)
        {
            if (await _recipeIngredientRepository.FindByIngredientIdAsync(id) != null)
                throw new IngredientUsedInRecipeException();
        }
        #endregion
    }
}
