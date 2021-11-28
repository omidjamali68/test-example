﻿using Cooking.Services.IngredientServices.Ingredients.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cooking.RestApi.Controllers.V1.Ingredients
{
    [ApiVersion("1.0")]
    [ApiController, Route("/api/v{version:apiVersion}/ingredients")]
    [Authorize]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _service;

        public IngredientsController(IIngredientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Add(AddIngredientDto dto)
        {
            await _service.AddAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(long id, UpdateIngredientDto dto)
        {
            await _service.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _service.DeleteAsync(id);
        }
    }
}
