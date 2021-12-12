using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Application;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.Ingredients.IngredientUnits.Contracts
{
    public interface IIngredientUnitRepository : IRepository
    {
        Task<IngredientUnit> FindByIdAsync(int id);
        Task<PageResult<GetAllIngredientUnitDto>> GetAll(
            string searchText, 
            Pagination pagination, 
            Sort<GetAllIngredientUnitDto> sortExpression);
    }
}
