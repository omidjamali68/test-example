using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;
using Cooking.Services.Ingredients.IngredientUnits.Contracts;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;

namespace Cooking.Services.IngredientServices.IngredientUnits
{
    public class IngredientUnitAppService : IIngredientUnitService
    {
        private readonly IIngredientUnitRepository _repository;

        public IngredientUnitAppService(IIngredientUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<PageResult<GetAllIngredientUnitDto>> GetAll(
            string searchText,
            Pagination pagination, 
            Sort<GetAllIngredientUnitDto> sortExpression)
        {
            return await _repository.GetAll(searchText, pagination, sortExpression);
        }
    }
}