﻿using Cooking.Entities.Recipes;
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

        public async Task<bool> ExistInRecipe(long stepOperationId)
        {
            return await _context.RecipeSteps.AnyAsync(_ => _.StepOperationId == stepOperationId);
        }

        public async Task<StepOperation> FindById(long id)
        {
            return await _stepOperations.FindAsync(id);
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
    }
}
