﻿using System;

namespace Cooking.Entities.Recipes
{
    public class RecipeDocument
    {
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Guid DocumentId { get; set; }
        public string Extension { get; set; }
    }
}
