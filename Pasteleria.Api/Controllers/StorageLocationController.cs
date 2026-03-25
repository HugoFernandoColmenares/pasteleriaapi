using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.DTOs;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageLocationController : ControllerBase
    {
        private readonly IStorageLocationService _service;

        public StorageLocationController(IStorageLocationService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StorageLocationDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllStorageLocationsAsync();
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<IEnumerable<StorageLocationDto>>.SuccessResponse(result.Data, "Storage locations retrieved successfully."));
            }
            return BadRequest(ApiResponse<IEnumerable<StorageLocationDto>>.FailureResponse("Failed to retrieve storage locations.", result.Errors));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetStorageLocationByIdAsync(id);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<StorageLocationDto>.SuccessResponse(result.Data, "Storage location retrieved successfully."));
            }
            return NotFound(ApiResponse<StorageLocationDto>.FailureResponse($"Storage location with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateStorageLocationDto dto)
        {
            var result = await _service.CreateStorageLocationAsync(dto);
            if (result.IsSuccessful)
            {
                var response = ApiResponse<StorageLocationDto>.SuccessResponse(result.Data, "Storage location created successfully.", (int)HttpStatusCode.Created);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, response);
            }
            return BadRequest(ApiResponse<StorageLocationDto>.FailureResponse("Failed to create storage location.", result.Errors));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<StorageLocationDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] StorageLocationDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(ApiResponse<StorageLocationDto>.FailureResponse("ID in URL does not match ID in body."));
            }

            var result = await _service.UpdateStorageLocationAsync(dto);
            if (result.IsSuccessful)
            {
                return Ok(ApiResponse<StorageLocationDto>.SuccessResponse(result.Data, "Storage location updated successfully."));
            }
            return BadRequest(ApiResponse<StorageLocationDto>.FailureResponse("Failed to update storage location.", result.Errors));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteStorageLocationAsync(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            return NotFound(ApiResponse<bool>.FailureResponse($"Storage location with ID {id} not found.", result.Errors, (int)HttpStatusCode.NotFound));
        }
    }
}
