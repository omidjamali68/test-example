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
    }
}
