﻿using System;
using System.Collections.Generic;
using Cooking.Entities.Documents;
using Cooking.Entities.Ingredients;
using Cooking.Entities.Recipes;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.TestTools.RecipeTestTools.RecipeDocuments;
using Cooking.TestTools.RecipeTestTools.RecipeIngredients;
using Cooking.TestTools.RecipeTestTools.RecipeSteps;

namespace Cooking.TestTools.RecipeTestTools.Recipes
{
    public class RecipeBuilder
    {
        private Recipe _recipe = new Recipe { 
            FoodName = "dummy_foodName",
        };

        public RecipeBuilder(int nationalityId, int recipeCategoryId)
        {
            _recipe.NationalityId = nationalityId;
            _recipe.RecipeCategoryId = recipeCategoryId;
        }

        public RecipeBuilder WithIngredient(Ingredient ingredient)
        {
            _recipe.RecipeIngredients.Add(RecipeIngredientFactory.CreateInstance(ingredient.Id));
            return this;
        }

        public RecipeBuilder WithStep(StepOperation stepOperation)
        {
            _recipe.RecipeSteps.Add(RecipeStepFactory.CreateInstance(stepOperation.Id));
            return this;
        }

        public RecipeBuilder WithDocument(Document otherDocument)
        {
            _recipe.RecipeDocuments
                .Add(RecipeDocumentFactory.CreateInstance(otherDocument.Id, otherDocument.Extension));
            return this;
        }

        public RecipeBuilder WithFoodName(string foodName)
        {
            _recipe.FoodName = foodName;
            return this;
        }

        public RecipeBuilder WithMainDocument(Document mainDocument)
        {
            _recipe.MainDocumentId = mainDocument.Id;
            _recipe.MainDocumentExtension = mainDocument.Extension;
            return this;
        }

        public Recipe Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Recipes.Add(_recipe));
            return _recipe;
        }

        
    }
}
