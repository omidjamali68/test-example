using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Persistence.EF.RecipePersistence.Recipes
{
    public class EFRecipeRepository : IRecipeRepository
    {
        private readonly DbSet<Recipe> _recipes;
        public EFRecipeRepository(EFDataContext context)
        {
            _recipes = context.Recipes;
        }
    }
}
