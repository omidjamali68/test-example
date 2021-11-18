using Cooking.Entities.Ingredients;
using Cooking.Services.Ingredients.IngredientUnits.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.IngredientPersistence.IngredientUnits
{
    public class EFIngredientUnitRepository : IIngredientUnitRepository
    {
        private readonly EFDataContext _context;
        private readonly DbSet<IngredientUnit> _ingredientUnits;
        public EFIngredientUnitRepository(EFDataContext context)
        {
            _context = context;
            _ingredientUnits = _context.Set<IngredientUnit>();
        }

        public async Task<IngredientUnit> FindByIdAsync(int id)
        {
            return await _ingredientUnits.FindAsync(id);
        }
    }
}
