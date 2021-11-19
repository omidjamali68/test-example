using Cooking.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Services.RecipeServices.StepOperations.Contracts
{
    public interface IStepOperationService : IService
    {
        Task<long> AddAsync(AddStepOperationDto dto);
    }
}
