using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.TestTools.DocumentTestTools;

namespace Cooking.TestTools.IngredientTestTools.Ingredients
{
    public class IngredientBuilder
    {
        private Ingredient _ingredient = new Ingredient();

        public IngredientBuilder(int ingredientUnitId, Document document)
        {
            _ingredient.IngredientUnitId = ingredientUnitId;
            _ingredient.AvatarId = document.Id;
            _ingredient.Extension = document.Extension;
        }

        public IngredientBuilder WithTitle(string title)
        {
            _ingredient.Title = title;
            return this;
        }

        public Ingredient Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Ingredients.Add(_ingredient));
            return _ingredient;
        }
    }
}
