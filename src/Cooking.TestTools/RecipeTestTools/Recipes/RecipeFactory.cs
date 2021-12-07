<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.Recipes.Contracts;
=======
﻿using Cooking.Persistence.EF;
using Cooking.Persistence.EF.RecipePersistence.Recipes;
using Cooking.Services.RecipeServices.Recipes;
using System;
using System.Collections.Generic;
using System.Text;
>>>>>>> 4182b362f4e20828d0a3026c8db6533e07b6d344

namespace Cooking.TestTools.RecipeTestTools.Recipes
{
    public static class RecipeFactory
    {
        public static RecipeAppService CreateService(EFDataContext context)
        {
<<<<<<< HEAD
            var unitOfWork = new EFUnitOfWork(context);
            return new RecipeAppService(unitOfWork);
        }
    }
}
=======
            var repository = new EFRecipeRepository(context);
            var unitOfWork = new EFUnitOfWork(context);

            return new RecipeAppService(repository, unitOfWork);
        }
    }
}
>>>>>>> 4182b362f4e20828d0a3026c8db6533e07b6d344
