using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.Recipes.Contracts;

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

        #region Helper Methods

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
        
    }
}
