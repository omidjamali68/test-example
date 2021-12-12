using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.IngredientServices.IngredientUnits.Contracts
{
    public interface IIngredientUnitService : IService
    {
        Task<PageResult<GetAllIngredientUnitDto>> GetAll(
            string searchText,
            Pagination pagination,
            Sort<GetAllIngredientUnitDto> sortExpression);
    }
}