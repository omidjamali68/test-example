using System.Collections.Generic;
using System.Threading.Tasks;
using Cooking.Services.RecipeServices.RecipeCtegories.Contracts;

namespace Cooking.Services.RecipeServices.RecipeCtegories
{
    public class RecipeCategoryAppService : IRecipeCategoryService
    {
        private readonly IRecipeCategoryRepository _repository;

        public RecipeCategoryAppService(IRecipeCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<GetAllRecipeCategoryDto>> GetAll()
        {
            return await _repository.GetAll();
        }

    }
}