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
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// Retrieves all recipes.
        /// </summary>
        /// <returns>A list of recipes.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListRecipeDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _recipeService.GetAllRecipesAsync(pageNumber, pageSize);
            if (result.IsSuccessful && result.Data != null)
            {
                var pagination = MapToPaginationDto(result.Data);
                return Ok(ApiResponse<List<ListRecipeDto>>.SuccessResponse(result.Data.Items, "Recipes retrieved successfully.", (int)HttpStatusCode.OK, pagination));
            }
            return BadRequest(ApiResponse<List<ListRecipeDto>>.FailureResponse("Failed to retrieve recipes.", result.Errors));
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
        /// Retrieves a recipe by its ID.
        /// </summary>
        /// <param name="id">The recipe ID.</param>
        /// <returns>The recipe details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _recipeService.GetRecipeByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<RecipeDto>.SuccessResponse(result.Data, "Recipe retrieved successfully."));
            }
            return NotFound(ApiResponse<RecipeDto>.FailureResponse($"Recipe with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="recipeDto">The recipe creation data.</param>
        /// <returns>The created recipe.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateRecipeDto recipeDto)
        {
            var result = await _recipeService.AddRecipeAsync(recipeDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<RecipeDto>.SuccessResponse(result.Data, "Recipe created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<RecipeDto>.FailureResponse("Failed to create recipe.", result.Errors));
        }

        /// <summary>
        /// Updates an existing recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe to update.</param>
        /// <param name="recipeDto">The updated recipe data.</param>
        /// <returns>The updated recipe.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RecipeDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecipeDto recipeDto)
        {
            if (id != recipeDto.Id)
            {
                return BadRequest(ApiResponse<RecipeDto>.FailureResponse("Recipe ID in URL does not match Recipe ID in body."));
            }

            var result = await _recipeService.UpdateRecipeAsync(recipeDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<RecipeDto>.SuccessResponse(result.Data, "Recipe updated successfully."));
            }
            return BadRequest(ApiResponse<RecipeDto>.FailureResponse("Failed to update recipe.", result.Errors));
        }

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _recipeService.DeleteRecipeAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Recipe with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
