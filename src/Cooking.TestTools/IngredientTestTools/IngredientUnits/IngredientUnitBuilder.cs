using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.TestTools.IngredientTestTools.IngredientUnits
{
    public class IngredientUnitBuilder
    {
        private IngredientUnit _ingredientUnit = new IngredientUnit();

        public IngredientUnitBuilder WithTitle(string title)
        {
            _ingredientUnit.Title = title;
            return this;
        }

        public IngredientUnit Build(EFDataContext context)
        {
            context.Manipulate(_ => _.IngredientUnits.Add(_ingredientUnit));
            return _ingredientUnit;
        }
    }
}
