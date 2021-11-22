using Cooking.Entities.Recipes;
using Cooking.Services.RecipeServices.StepOperations.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<StepOperation> FindById(long id)
        {
            return await _stepOperations.FindAsync(id);
        }
    }
}
