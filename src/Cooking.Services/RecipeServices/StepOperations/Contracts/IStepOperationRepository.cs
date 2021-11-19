﻿using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public interface IStepOperationRepository : IRepository
    {
        Task AddAsync(StepOperation stepOperation);
    }
}