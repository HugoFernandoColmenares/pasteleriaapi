using Pasteleria.Shared.Auth.Auxiliar;
using Pasteleria.Shared.Auth.Dtos;
using Microsoft.AspNetCore.Mvc;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.Extensions;
using System.Net;

namespace Pasteleria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <returns>The registration result.</returns>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            var baseUrl = "https://pasteleriacic.github.io/pasteleriaapp/#/";
            var result = await _authService.RegisterAsync(request, baseUrl);
            if (result.Result)
            {
                return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "User registered successfully. Please check your email for confirmation."));
            }
            return BadRequest(ApiResponse<AuthResult>.FailureResponse("Registration failed.", result.Errors));
        }

        /// <summary>
        /// Initiates the forgot password process.
        /// </summary>
        /// <param name="request">The user email.</param>
        /// <returns>The result of the verification.</returns>
        [HttpPost("ForgotPassword")]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForggotPasswordRequestDto request)
        {
            var result = await _authService.ForgotPasswordAsync(request);
            if (result.Result)
            {
                return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Email verified. You can now reset your password."));
            }
            return BadRequest(ApiResponse<AuthResult>.FailureResponse("Email verification failed.", result.Errors));
        }

        /// <summary>
        /// Resets the user password.
        /// </summary>
        /// <param name="model">The reset details.</param>
        /// <returns>The result of the operation.</returns>
        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
        {
            var result = await _authService.ResetPasswordAsync(model);
            if (result.Result)
            {
                return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Password has been successfully reset."));
            }
            return BadRequest(ApiResponse<AuthResult>.FailureResponse("Failed to reset password.", result.Errors));
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="dto">The login credentials.</param>
        /// <returns>The authentication token.</returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result.Result)
            {
                return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Login successful."));
            }
            return BadRequest(ApiResponse<AuthResult>.FailureResponse("Login failed. Please check your credentials.", result.Errors));
        }

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        /// <param name="confirmEmailDto">The confirmation details.</param>
        /// <returns>The result of the confirmation.</returns>
        [HttpPost("ConfirmEmail")]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            var result = await _authService.ConfirmEmailAsync(confirmEmailDto.UserId, confirmEmailDto.Code);
            if (result.Result)
            {
                return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Email confirmed successfully."));
            }
            return BadRequest(ApiResponse<AuthResult>.FailureResponse("Failed to confirm email.", result.Errors));
        }
    }
}
