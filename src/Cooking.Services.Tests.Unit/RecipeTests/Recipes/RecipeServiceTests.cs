using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.TestTools.RecipeTestTools.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.Tests.Unit.RecipeTests.Recipes
{
    public class RecipeServiceTests
    {
        private readonly IRecipeService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        public RecipeServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            _sut = RecipeFactory.CreateService(_context);
        }
    }
}
