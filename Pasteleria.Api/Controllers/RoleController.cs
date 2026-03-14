using Pasteleria.Shared.Auth.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all identity roles.
        /// </summary>
        /// <returns>A list of roles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<IdentityRole>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<IdentityRole>>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles != null && roles.Count > 0)
            {
                return Ok(ApiResponse<List<IdentityRole>>.SuccessResponse(roles, "Roles retrieved successfully."));
            }
            return NotFound(ApiResponse<List<IdentityRole>>.FailureResponse("No roles found.", null, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Retrieves role details by its ID.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The role details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetails(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                return Ok(ApiResponse<IdentityRole>.SuccessResponse(role, "Role details retrieved successfully."));
            }
            return NotFound(ApiResponse<IdentityRole>.FailureResponse($"Role with ID {id} not found.", null, (int)HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Creates a new identity role.
        /// </summary>
        /// <param name="dto">The role details.</param>
        /// <returns>The creation result.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] RoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<IdentityRole>.FailureResponse("Invalid model state.", errors));
            }

            var role = new IdentityRole()
            {
                Name = dto.Name,
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetDetails), new { id = role.Id }, ApiResponse<IdentityRole>.SuccessResponse(role, "Role created successfully.", (int)HttpStatusCode.Created));
            }

            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(ApiResponse<IdentityRole>.FailureResponse("Failed to create role.", identityErrors));
        }

        /// <summary>
        /// Updates an existing identity role.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <param name="updateDto">The updated role data.</param>
        /// <returns>The update result.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IdentityRole>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody] RoleDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<IdentityRole>.FailureResponse("Invalid model state.", errors));
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound(ApiResponse<IdentityRole>.FailureResponse($"Role with ID {id} not found.", null, (int)HttpStatusCode.NotFound));
            }

            existingRole.Name = updateDto.Name;
            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return Ok(ApiResponse<IdentityRole>.SuccessResponse(existingRole, "Role updated successfully."));
            }

            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(ApiResponse<IdentityRole>.FailureResponse("Failed to update role.", identityErrors));
        }

        /// <summary>
        /// Deletes an identity role.
        /// </summary>
        /// <param name="id">The role ID to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Role with ID {id} not found.", null, (int)HttpStatusCode.NotFound));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }

            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(ApiResponse<bool>.FailureResponse("Failed to delete role.", identityErrors));
        }
    }
}
