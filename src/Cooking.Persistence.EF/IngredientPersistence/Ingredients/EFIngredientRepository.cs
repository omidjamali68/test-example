using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.IngredientPersistence.Ingredients
{
    public class EFIngredientRepository : IIngredientRepository
    {
        private readonly DbSet<Ingredient> _ingredients;

        public EFIngredientRepository(EFDataContext context)
        {
            _ingredients = context.Ingredients;
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            await _ingredients.AddAsync(ingredient);
        }

        public async Task<Ingredient> FindByIdAsync(long id)
        {
            return await _ingredients.FindAsync(id);
        }

        public async Task<bool> IsTitleAndUnitExistAsync(string title, int ingredientUnitId, long? id)
        {
            return await _ingredients
                .AnyAsync(_ => _.Title.Replace(" ", "").Equals(title.Replace(" ", "")) 
                && _.IngredientUnitId == ingredientUnitId
                && _.Id != id);
        }

        public void Remove(Ingredient ingredient)
        {
            _ingredients.Remove(ingredient);
        }

        public async Task<PageResult<GetAllIngredientDto>> GetAllAsync(string searchText, Pagination pagination, Sort<GetAllIngredientDto> sortExpression)
        {
            var results = _ingredients.Select(_ => new GetAllIngredientDto
            {
                Id = _.Id,
                IngredientUnitTitle = _.IngredientUnit.Title,
                Title = _.Title,
                AvatarId = _.AvatarId,
                Extension = _.Extension,
            });

            if (searchText != null)
                results = FilterSearchText(searchText, results);

            if (sortExpression != null) results = results.Sort(sortExpression);

            PageResult<GetAllIngredientDto> pageResult = null;

            if (pagination != null)
            {
                pageResult = results.PageResult(pagination);
            }
            else
            {
                var resultList = await results?.ToListAsync();
                if (resultList != null) 
                    pageResult = new PageResult<GetAllIngredientDto>(resultList, resultList.Count);
            }
            return pageResult;
        }

        #region Helper Methods

        private IQueryable<GetAllIngredientDto> FilterSearchText(
            string searchText,
            IQueryable<GetAllIngredientDto> results
            )
        {
            return results
                .Where(_ => _.Title.Replace(" ", "").Contains(searchText.Replace(" ", "")) ||
                _.IngredientUnitTitle.Replace(" ", "").Contains(searchText.Replace(" ", "")));
        }

        #endregion
    }
}
