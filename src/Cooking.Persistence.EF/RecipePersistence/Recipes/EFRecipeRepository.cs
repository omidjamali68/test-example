using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.RecipePersistence.Recipes
{
    public class EFRecipeRepository : IRecipeRepository
    {
        private readonly DbSet<Recipe> _recipes;
        public EFRecipeRepository(EFDataContext context)
        {
            _recipes = context.Recipes;
        }

        public async Task Add(Recipe recipe)
        {
            await _recipes.AddAsync(recipe);
        }

        public async Task<Recipe> FindByIdAsync(long id)
        {
            return await _recipes.FindAsync(id);
        }

        public async Task<GetRecipeDto> GetAsync(long id)
        {
            return await _recipes.Where(_ => _.Id == id)
                .Select(_ => new GetRecipeDto
                {
                    Id = _.Id,
                    FoodName = _.FoodName,
                    Duration = _.Duration,
                    NationalityId = _.NationalityId,
                    RecipeCategoryId = _.RecipeCategoryId,
                    RecipeDocuments = _.RecipeDocuments.Any()
                    ? _.RecipeDocuments.Select(_ => new RecipeDocumentDto
                    {
                        DocumentId = _.DocumentId,
                        Extension = _.Extension
                    }).ToHashSet()
                    : default,
                    RecipeIngredients = _.RecipeIngredients.Any()
                    ? _.RecipeIngredients.Select(_ => new RecipeIngredientDto
                    {
                        IngredientId = _.IngredientId,
                        Quantity = _.Quantity
                    }).ToHashSet()
                    : default,
                    RecipeSteps = _.RecipeSteps.Any()
                    ? _.RecipeSteps.Select(_ => new RecipeStepDto
                    {
                        StepOperationId = _.StepOperationId,
                        Description = _.Description,
                        Order = _.Order
                    }).ToHashSet()
                    : default
                }).SingleOrDefaultAsync();
        }

        public void Remove(Recipe recipe)
        {
            _recipes.Remove(recipe);
        }
    }
}
