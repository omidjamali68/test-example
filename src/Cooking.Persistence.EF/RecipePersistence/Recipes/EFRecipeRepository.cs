using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            return await _recipes.Include(_ => _.RecipeDocuments)
                .Include(_ => _.RecipeIngredients)
                .Include(_ => _.RecipeSteps)
                .FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<PageResult<GetAllRecipeDto>> GetAllAsync(
            string searchText,
            Pagination pagination,
            Sort<GetAllRecipeDto> sortExpression)
        {
            var results = _recipes.Select(_ => new GetAllRecipeDto
            {
                Id = _.Id,
                FoodName = _.FoodName,
                Duration = _.Duration,
                RecipeCategoryTitle = _.RecipeCategory.Title,
                MainDocumentId = _.MainDocumentId,
                MainDocumentExtension = _.MainDocumentExtension,
                NationalityName = _.Nationality.Name
            });

            if (searchText != null)
                results = FilterSearchText(searchText, results);

            if (sortExpression != null) results = results.Sort(sortExpression);

            PageResult<GetAllRecipeDto> pageResult = null;

            if (pagination != null)
            {
                pageResult = results.PageResult(pagination);
            }
            else
            {
                var resultList = await results?.ToListAsync();
                if (resultList != null)
                    pageResult = new PageResult<GetAllRecipeDto>(
                        resultList,
                        resultList.Count);
            }
            return pageResult;
        }

        public async Task<ICollection<GetAllRecipeDto>> GetAllByNationalityIdAsync(int nationalityId)
        {
            return await _recipes.Where(_ => _.NationalityId == nationalityId)
                .Select(_ => new GetAllRecipeDto
                {
                    Id = _.Id,
                    FoodName = _.FoodName,
                    Duration = _.Duration,
                    RecipeCategoryTitle = _.RecipeCategory.Title,
                    MainDocumentId = _.MainDocumentId,
                    MainDocumentExtension = _.MainDocumentExtension,
                    NationalityName = _.Nationality.Name
                }).ToListAsync();
        }

        public async Task<GetRecipeDto> GetAsync(long id)
        {
            return await _recipes.Where(_ => _.Id == id)
                .Select(_ => new GetRecipeDto
                {
                    FoodName = _.FoodName,
                    Duration = _.Duration,
                    NationalityId = _.NationalityId,
                    RecipeCategoryId = _.RecipeCategoryId,
                    MainDocumentId = _.MainDocumentId,
                    MainDocumentExtension = _.MainDocumentExtension,
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

        public async Task<IList<GetRandomRecipesForHomePageDto>> GetRandomForHomePage()
        {
            return await _recipes.Take(10)
                .Select(_ => new GetRandomRecipesForHomePageDto
                {
                    FoodName = _.FoodName,
                    Duration = _.Duration,
                    MainDocumentId = _.MainDocumentId,
                    MainDocumentExtension = _.MainDocumentExtension
                }).ToListAsync();
        }

        public void Remove(Recipe recipe)
        {
            _recipes.Remove(recipe);
        }

        #region Helper Methods

        private IQueryable<GetAllRecipeDto> FilterSearchText(
            string searchText,
            IQueryable<GetAllRecipeDto> results
            )
        {
            return results
                .Where(_ =>
                _.FoodName.Replace(" ", "").Contains(searchText.Replace(" ", "")) ||
                _.RecipeCategoryTitle.Replace(" ", "").Contains(searchText.Replace(" ", "")) ||
                _.NationalityName.Replace(" ", "").Contains(searchText.Replace(" ", "")));
        }

        #endregion
    }
}
