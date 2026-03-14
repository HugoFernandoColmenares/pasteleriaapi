using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService)
        {
            _recipeIngredientService = recipeIngredientService;
        }

        /// <summary>
        /// Retrieves all recipe ingredients.
        /// </summary>
        /// <returns>A list of recipe ingredients.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListRecipeIngredientDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _recipeIngredientService.GetAllRecipeIngredientsAsync();
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<List<ListRecipeIngredientDto>>.SuccessResponse(result.Data, "Recipe ingredients retrieved successfully."));
            }
            return BadRequest(ApiResponse<List<ListRecipeIngredientDto>>.FailureResponse("Failed to retrieve recipe ingredients.", result.Errors));
        }

        /// <summary>
        /// Retrieves a recipe ingredient by its ID.
        /// </summary>
        /// <param name="id">The recipe ingredient ID.</param>
        /// <returns>The recipe ingredient details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _recipeIngredientService.GetRecipeIngredientByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<RecipeIngredientDto>.SuccessResponse(result.Data, "Recipe ingredient retrieved successfully."));
            }
            return NotFound(ApiResponse<RecipeIngredientDto>.FailureResponse($"Recipe ingredient with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Adds a new ingredient to a recipe.
        /// </summary>
        /// <param name="recipeIngredientDto">The recipe ingredient creation data.</param>
        /// <returns>The created recipe ingredient.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateRecipeIngredientDto recipeIngredientDto)
        {
            var result = await _recipeIngredientService.AddRecipeIngredientAsync(recipeIngredientDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<RecipeIngredientDto>.SuccessResponse(result.Data, "Recipe ingredient created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<RecipeIngredientDto>.FailureResponse("Failed to create recipe ingredient.", result.Errors));
        }

        /// <summary>
        /// Updates an existing recipe ingredient relationship.
        /// </summary>
        /// <param name="id">The ID of the relationship to update.</param>
        /// <param name="recipeIngredientDto">The updated data.</param>
        /// <returns>The updated recipe ingredient.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RecipeIngredientDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecipeIngredientDto recipeIngredientDto)
        {
            if (id != recipeIngredientDto.Id)
            {
                return BadRequest(ApiResponse<RecipeIngredientDto>.FailureResponse("RecipeIngredient ID in URL does not match ID in body."));
            }

            var result = await _recipeIngredientService.UpdateRecipeIngredientAsync(recipeIngredientDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<RecipeIngredientDto>.SuccessResponse(result.Data, "Recipe ingredient updated successfully."));
            }
            return BadRequest(ApiResponse<RecipeIngredientDto>.FailureResponse("Failed to update recipe ingredient.", result.Errors));
        }

        /// <summary>
        /// Deletes an ingredient from a recipe.
        /// </summary>
        /// <param name="id">The ID of the relationship to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _recipeIngredientService.DeleteRecipeIngredientAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Recipe ingredient with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
