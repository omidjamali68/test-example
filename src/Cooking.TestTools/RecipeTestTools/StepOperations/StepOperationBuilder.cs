using Cooking.Entities.Documents;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.TestTools.RecipeTestTools.StepOperations
{
    public class StepOperationBuilder
    {
        private StepOperation _stepOperation = new StepOperation();

        public StepOperationBuilder(Document document)
        {
            _stepOperation.AvatarId = document.Id;
            _stepOperation.Extension = document.Extension;
        }

        public StepOperationBuilder WithTitle(string title)
        {
            _stepOperation.Title = title;
            return this;
        }

        public StepOperationBuilder WithAvatar(Document document)
        {
            _stepOperation.AvatarId = document.Id;
            _stepOperation.Extension = document.Extension;
            return this;
        }

        public StepOperation Build(EFDataContext context)
        {
            context.Manipulate(_ => _.StepOperations.Add(_stepOperation));
            return _stepOperation;
        }

        
    }
}
