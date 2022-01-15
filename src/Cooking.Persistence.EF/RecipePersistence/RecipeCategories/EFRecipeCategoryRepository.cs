using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Test;
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

        public async Task<IList<GetAllRecipeCategoryDto>> GetAll(string searchText)
        {
            var result = _recipeCategories.Select(_ => new GetAllRecipeCategoryDto{
                Id = _.Id,
                Title = _.Title
            });

            if (!string.IsNullOrWhiteSpace(searchText))
                result = SearchForText(searchText, result);

            return await result.ToListAsync();
        }

        private IQueryable<GetAllRecipeCategoryDto> SearchForText(
            string searchText, 
            IQueryable<GetAllRecipeCategoryDto> result)
        {
            return result.Where(_ => _.Title.ToLower().Replace(" ", "")
                .Contains(searchText.ToLower().Replace(" ", "")));
        }
    }
}