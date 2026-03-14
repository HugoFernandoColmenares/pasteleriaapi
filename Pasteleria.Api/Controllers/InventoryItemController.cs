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
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        /// <summary>
        /// Retrieves all inventory items.
        /// </summary>
        /// <returns>A list of inventory items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListInventoryItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _inventoryItemService.GetAllInventoryItemsAsync(pageNumber, pageSize);
            if (result.IsSuccessful && result.Data != null)
            {
                var pagination = MapToPaginationDto(result.Data);
                return Ok(ApiResponse<List<ListInventoryItemDto>>.SuccessResponse(result.Data.Items, "Inventory items retrieved successfully.", (int)HttpStatusCode.OK, pagination));
            }
            return BadRequest(ApiResponse<List<ListInventoryItemDto>>.FailureResponse("Failed to retrieve inventory items.", result.Errors));
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
        /// Retrieves an inventory item by its ID.
        /// </summary>
        /// <param name="id">The inventory item ID.</param>
        /// <returns>The inventory item details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _inventoryItemService.GetInventoryItemByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(result.Data, "Inventory item retrieved successfully."));
            }
            return NotFound(ApiResponse<InventoryItemDto>.FailureResponse($"Inventory item with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Adds a new item to the inventory.
        /// </summary>
        /// <param name="inventoryItemDto">The inventory item creation data.</param>
        /// <returns>The created inventory item.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateInventoryItemDto inventoryItemDto)
        {
            var result = await _inventoryItemService.AddInventoryItemAsync(inventoryItemDto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<InventoryItemDto>.SuccessResponse(result.Data, "Inventory item created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<InventoryItemDto>.FailureResponse("Failed to create inventory item.", result.Errors));
        }

        /// <summary>
        /// Updates an existing inventory item.
        /// </summary>
        /// <param name="id">The ID of the item to update.</param>
        /// <param name="inventoryItemDto">The updated inventory item data.</param>
        /// <returns>The updated inventory item.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] InventoryItemDto inventoryItemDto)
        {
            if (id != inventoryItemDto.Id)
            {
                return BadRequest(ApiResponse<InventoryItemDto>.FailureResponse("InventoryItem ID in URL does not match ID in body."));
            }

            var result = await _inventoryItemService.UpdateInventoryItemAsync(inventoryItemDto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(result.Data, "Inventory item updated successfully."));
            }
            return BadRequest(ApiResponse<InventoryItemDto>.FailureResponse("Failed to update inventory item.", result.Errors));
        }

        /// <summary>
        /// Deletes an inventory item.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _inventoryItemService.DeleteInventoryItemAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Inventory item with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
