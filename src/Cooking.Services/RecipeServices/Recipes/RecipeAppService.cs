using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.Recipes.Exceptions;

namespace Cooking.Services.RecipeServices.Recipes
{
    public class RecipeAppService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeAppService(IRecipeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Add(AddRecipeDto dto)
        {
            Recipe recipe = ExchangeAddDtoWithEntity(dto);

            await _repository.Add(recipe);

            await _unitOfWork.CompleteAsync();

            return recipe.Id;
        }

        public async Task DeleteAsync(long id)
        {
            var recipe = await _repository.FindByIdAsync(id);
            GuardAgainstRecipeNotFound(recipe);

            _repository.Remove(recipe);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(UpdateRecipeDto dto, long id)
        {
            var recipe = await _repository.FindByIdAsync(id);
            GuardAgainstRecipeNotFound(recipe);
            ExchangeEntityWithUpdateDto(dto, recipe);

            await _unitOfWork.CompleteAsync();
        }
        
        public async Task<GetRecipeDto> GetAsync(long id)
        {
            return await _repository.GetAsync(id);
        }


        #region Helper Methods

        private static void ExchangeEntityWithUpdateDto(UpdateRecipeDto dto, Recipe recipe)
        {
            recipe.Duration = dto.Duration;
            recipe.FoodName = dto.FoodName;
            recipe.NationalityId = dto.NationalityId;
            recipe.RecipeCategoryId = dto.RecipeCategoryId;
            recipe.RecipeDocuments = dto.RecipeDocuments.Select(_ => new RecipeDocument
            {
                DocumentId = _.DocumentId,
                Extension = _.Extension,
            }).ToHashSet();
            recipe.RecipeIngredients = dto.RecipeIngredients.Select(_ => new RecipeIngredient
            {
                IngredientId = _.IngredientId,
                Quantity = _.Quantity
            }).ToHashSet();
            recipe.RecipeSteps = dto.RecipeSteps.Select(_ => new RecipeStep
            {
                Description = _.Description,
                Order = _.Order,
                StepOperationId = _.StepOperationId
            }).ToHashSet();
        }
        
        private static Recipe ExchangeAddDtoWithEntity(AddRecipeDto dto)
        {
            return new Recipe
            {
                Duration = dto.Duration,
                FoodName = dto.FoodName,
                NationalityId = dto.NationalityId,
                RecipeCategoryId = dto.RecipeCategoryId,
                RecipeDocuments = dto.RecipeDocuments.Select(_ => new RecipeDocument
                {
                    DocumentId = _.DocumentId,
                    Extension = _.Extension,
                }).ToHashSet(),
                RecipeIngredients = dto.RecipeIngredients.Select(_ => new RecipeIngredient
                {
                    IngredientId = _.IngredientId,
                    Quantity = _.Quantity
                }).ToHashSet(),
                RecipeSteps = dto.RecipeSteps.Select(_ => new RecipeStep
                {
                    Description = _.Description,
                    Order = _.Order,
                    StepOperationId = _.StepOperationId
                }).ToHashSet()
            };
        }

        #endregion

        #region Guard Methods
        public void GuardAgainstRecipeNotFound(Recipe recipe)
        {
            _ = recipe ?? throw new RecipeNotFoundException();
        }

       

        #endregion
    }
}
