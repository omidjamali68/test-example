using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cooking.Services.RecipeServices.RecipeDocuments.Contracts;
using Cooking.Services.RecipeServices.RecipeIngredients.Contracts;
using Cooking.Services.RecipeServices.RecipeSteps.Contracts;

namespace Cooking.Services.RecipeServices.Recipes.Contracts
{
    public class UpdateRecipeDto
    {
        [Required]
        public string FoodName { get; set; }
        public short? Duration { get; set; }
        [Required]
        public int RecipeCategoryId { get; set; }
        [Required]
        public int NationalityId { get; set; }
        [Required]
        public Guid MainDocumentId { get; set; }
        [Required]
        [MaxLength(10)]
        public string MainDocumentExtension { get; set; }
        [Required]
        public HashSet<RecipeIngredientDto> RecipeIngredients { get; set; }
        [Required]
        public HashSet<RecipeDocumentDto> RecipeDocuments { get; set; }
        [Required]
        public HashSet<RecipeStepDto> RecipeSteps { get; set; }
    }
}