using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeCtegories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.RecipePersistence.RecipeCategories
{
    public class EFRecipeCategoryRepository : IRecipeCategoryRepository
    {
        private readonly DbSet<RecipeCategory> _recipeCategories;

        public EFRecipeCategoryRepository(EFDataContext context)
        {
            _recipeCategories = context.Set<RecipeCategory>();
        }

        public async Task<IList<GetAllRecipeCategoryDto>> GetAll()
        {
            return await _recipeCategories.Select(_ => new GetAllRecipeCategoryDto{
                Id = _.Id,
                Title = _.Title
            }).ToListAsync();
        }
    }
}