using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using Cooking.Services.Ingredients.IngredientUnits.Contracts;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.IngredientPersistence.IngredientUnits
{
    public class EFIngredientUnitRepository : IIngredientUnitRepository
    {
        private readonly DbSet<IngredientUnit> _ingredientUnits;
        public EFIngredientUnitRepository(EFDataContext context)
        {
            _ingredientUnits = context.IngredientUnits;
        }

        public async Task<IngredientUnit> FindByIdAsync(int id)
        {
            return await _ingredientUnits.FindAsync(id);
        }

        public async Task<PageResult<GetAllIngredientUnitDto>> GetAll(
            string searchText, 
            Pagination pagination, 
            Sort<GetAllIngredientUnitDto> sortExpression)
        {
            var results = _ingredientUnits.Select(_ => new GetAllIngredientUnitDto{
                Id = _.Id,
                Title = _.Title
            });

            if (searchText != null)
                results = FilterSearchText(searchText, results);

            if (sortExpression != null) results = results.Sort(sortExpression);

            PageResult<GetAllIngredientUnitDto> pageResult = null;

            if (pagination != null)
            {
                pageResult = results.PageResult(pagination);
            }
            else
            {
                var resultList = await results?.ToListAsync();
                if (resultList != null) 
                    pageResult = new PageResult<GetAllIngredientUnitDto>(resultList, resultList.Count);
            }
            
            return pageResult;
        }

        #region Helper Methods

        private IQueryable<GetAllIngredientUnitDto> FilterSearchText(
            string searchText,
            IQueryable<GetAllIngredientUnitDto> results
            )
        {
            return results
                .Where(_ => _.Title.Replace(" ", "").Contains(searchText.Replace(" ", "")));
        }

        #endregion
    }
}
