using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Services.RecipeServices.RecipeCtegories.Contracts
{
    public interface IRecipeCategoryRepository : IRepository
    {
        Task<IList<GetAllRecipeCategoryDto>> GetAll(string searchText);
    }
}