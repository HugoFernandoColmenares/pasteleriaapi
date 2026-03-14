using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Retrieves all ingredients.
        /// </summary>
        /// <returns>A list of ingredients.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListIngredientDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _ingredientService.GetAllIngredientsAsync(pageNumber, pageSize);
            if (result.IsSuccessful && result.Data != null)
            {
                var pagination = MapToPaginationDto(result.Data);
                return Ok(ApiResponse<List<ListIngredientDto>>.SuccessResponse(result.Data.Items, "Ingredients retrieved successfully.", (int)HttpStatusCode.OK, pagination));
            }
            return BadRequest(ApiResponse<List<ListIngredientDto>>.FailureResponse("Failed to retrieve ingredients.", result.Errors));
        }

        private PaginationDto MapToPaginationDto<T>(PagedList<T> pagedList)
        {
            return new PaginationDto
            {
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                PageSize = pagedList.PageSize,
                TotalCount = pagedList.TotalCount,
                HasPrevious = pagedList.HasPrevious,
                HasNext = pagedList.HasNext
            };
        }

        /// <summary>
        /// Retrieves an ingredient by its ID.
        /// </summary>
        /// <param name="id">The ingredient ID.</param>
        /// <returns>The ingredient details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ingredientService.GetIngredientByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<IngredientDto>.SuccessResponse(result.Data, "Ingredient retrieved successfully."));
            }
            return NotFound(ApiResponse<IngredientDto>.FailureResponse($"Ingredient with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Creates a new ingredient.
        /// </summary>
        /// <param name="ingredientDto">The ingredient creation data.</param>
        /// <returns>The created ingredient.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateIngredientDto ingredientDto)
        {
            var result = await _ingredientService.AddIngredientAsync(ingredientDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<IngredientDto>.SuccessResponse(result.Data, "Ingredient created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<IngredientDto>.FailureResponse("Failed to create ingredient.", result.Errors));
        }

        /// <summary>
        /// Updates an existing ingredient.
        /// </summary>
        /// <param name="id">The ID of the ingredient to update.</param>
        /// <param name="ingredientDto">The updated ingredient data.</param>
        /// <returns>The updated ingredient.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IngredientDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] IngredientDto ingredientDto)
        {
            if (id != ingredientDto.Id)
            {
                return BadRequest(ApiResponse<IngredientDto>.FailureResponse("Ingredient ID in URL does not match Ingredient ID in body."));
            }

            var result = await _ingredientService.UpdateIngredientAsync(ingredientDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<IngredientDto>.SuccessResponse(result.Data, "Ingredient updated successfully."));
            }
            return BadRequest(ApiResponse<IngredientDto>.FailureResponse("Failed to update ingredient.", result.Errors));
        }

        /// <summary>
        /// Deletes an ingredient by its ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _ingredientService.DeleteIngredientAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Ingredient with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
