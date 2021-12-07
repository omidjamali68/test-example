using Cooking.Entities.Recipes;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.RecipeTestTools.Recipes;

namespace Cooking.Specs.RecipeTests.Recipes.Delete
{
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IRecipeService _sut;
        private Recipe _recipe;

        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = RecipeFactory.CreateService(_context);
        }
    }
}
