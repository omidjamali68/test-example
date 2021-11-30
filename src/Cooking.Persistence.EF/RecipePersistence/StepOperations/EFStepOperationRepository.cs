using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Persistence.EF.RecipePersistence.StepOperations
{
    public class EFStepOperationRepository : IStepOperationRepository
    {
        private readonly EFDataContext _context;
        private readonly DbSet<StepOperation> _stepOperations;
        public EFStepOperationRepository(EFDataContext context)
        {
            _context = context;
            _stepOperations = _context.Set<StepOperation>();
        }

        public async Task AddAsync(StepOperation stepOperation)
        {
            await _stepOperations.AddAsync(stepOperation);
        }

        public async Task<bool> ExistInRecipe(long stepOperationId)
        {
            return await _context.RecipeSteps.AnyAsync(_ => _.StepOperationId == stepOperationId);
        }

        public async Task<StepOperation> FindById(long id)
        {
            return await _stepOperations.FindAsync(id);
        }

        public async Task<PageResult<GetAllStepOperationDto>> GetAll(
            string searchText,
            Pagination pagination,
            Sort<GetAllStepOperationDto> sortExpression
            )
        {
            var results = _stepOperations.Select(_ => new GetAllStepOperationDto
            {
                AvatarId = _.AvatarId,
                Extension = _.Extension,
                Title = _.Title,
                Id = _.Id
            });

            if (searchText != null)
                results = ExecSearchTextFilter(searchText, results);

            if (sortExpression != null) results = results.Sort(sortExpression);

            PageResult<GetAllStepOperationDto>? pageResult = null;

            if (pagination != null)
            {
                pageResult = results.PageResult(pagination);
            }
            else
            {
                var resultList = await results?.ToListAsync();
                if (resultList != null) pageResult = new PageResult<GetAllStepOperationDto>(resultList, resultList.Count);
            }
            return pageResult;

        }

        public async Task<bool> IsTitleExist(string title, long? id)
        {
            return await _stepOperations.AnyAsync(_ => _.Title.Replace(" ", "").Equals(title.Replace(" ", "")) &&
                _.Id != id);
        }

        public void Remove(StepOperation stepOperation)
        {
            _stepOperations.Remove(stepOperation);
        }

        #region Helper Methods

        private IQueryable<GetAllStepOperationDto> ExecSearchTextFilter(
            string searchText,
            IQueryable<GetAllStepOperationDto> results
            )
        {
            return results.Where(_ => _.Title.Replace(" ", "").Contains(searchText.Replace(" ", "")));
        }

        #endregion
    }
}
