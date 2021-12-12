using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Persistence.EF;
using Cooking.Persistence.EF.IngredientPersistence.IngredientUnits;
using Cooking.Services.IngredientServices.IngredientUnits;
using Cooking.Services.IngredientServices.IngredientUnits.Contracts;

namespace Cooking.TestTools.IngredientTestTools.IngredientUnits
{
    public static class IngredientUnitFactory
    {
        public static IngredientUnitAppService CreateService(EFDataContext context)
        {
            var repository = new EFIngredientUnitRepository(context);
            return new IngredientUnitAppService(repository);
        }
    }
}