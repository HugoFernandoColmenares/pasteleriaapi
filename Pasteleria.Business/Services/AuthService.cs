using AutoMapper;
using Pasteleria.Business.Configuration;
using Pasteleria.Shared.Auth.Auxiliar;
using Pasteleria.Shared.Auth.Dtos;
using Pasteleria.Shared.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pasteleria.Business.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pasteleria.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;
        private readonly IEmailSender _emailSender;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IOptions<JwtConfig> jwtConfig, IEmailSender emailSender)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _jwtConfig = jwtConfig.Value;
            _emailSender = emailSender;
        }

        public async Task<AuthResult> RegisterAsync(UserRegistrationRequestDto request, string baseUrl)
        {
            var emailExist = await _authRepository.FindByEmailAsync(request.Email);
            if (emailExist != null)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Ya existe este correo." }
                };
            }

            var newUser = _mapper.Map<User>(request);
            var isCreated = await _authRepository.CreateUserAsync(newUser, request.Password);

            if (isCreated.Succeeded)
            {
                await _authRepository.AddToRoleAsync(newUser, "Visitor");
                await SendVerificationEmail(newUser, baseUrl);

                return new AuthResult { Result = true };
            }

            return new AuthResult
            {
                Result = false,
                Errors = isCreated.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> ForgotPasswordAsync(UserForggotPasswordRequestDto request)
        {
            var user = await _authRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "No existe ninguna cuenta asociada a este correo electrónico." }
                };
            }

            var token = await _authRepository.GeneratePasswordResetTokenAsync(user);
            return new AuthResult
            {
                Result = true,
                Token = token,
                Message = "Se ha verificado el correo, puede continuar con el proceso de restablecer la contraseña."
            };
        }

        public async Task<AuthResult> ResetPasswordAsync(ResetPasswordRequestDto model)
        {
            var user = await _authRepository.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "No existe ninguna cuenta asociada a este correo electrónico." }
                };
            }

            var result = await _authRepository.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new AuthResult { Result = true, Message = "La contraseña se ha restablecido correctamente." };
            }

            return new AuthResult
            {
                Result = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> LoginAsync(UserLoginRequestDto dto)
        {
            var existingUser = await _authRepository.FindByEmailAsync(dto.Email);
            if (existingUser == null || !await _authRepository.CheckPasswordAsync(existingUser, dto.Password))
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Credenciales inválidas." }
                };
            }

            if (!existingUser.EmailConfirmed)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Se necesita confirmar el correo electrónico." }
                };
            }

            var token = await GenerateTokenAsync(existingUser);
            return new AuthResult { Result = true, Token = token };
        }

        public async Task<AuthResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _authRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Usuario no encontrado." }
                };
            }

            if (user.EmailConfirmed == true)
            {
                return new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "El usuario ya valído su correo electrónico" }
                };
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _authRepository.ConfirmEmailAsync(user, decodedCode);

            return new AuthResult
            {
                Result = result.Succeeded,
                Message = result.Succeeded ? "Gracias por confirmar su correo electrónico." : "Ocurrió un error al confirmar el correo electrónico."
            };
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _authRepository.GetRolesAsync(user);
            var roleClaim = roles.FirstOrDefault() ?? "User";

            var claims = new List<Claim>
            {
                new Claim("id", user.Id!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new Claim("Role", roleClaim)
            };

            if (!string.IsNullOrEmpty(user.FirstName)) claims.Add(new Claim("FirstName", user.FirstName));
            if (!string.IsNullOrEmpty(user.LastName)) claims.Add(new Claim("LastName", user.LastName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task SendVerificationEmail(User user, string baseUrl)
        {
            var token = await _authRepository.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = $"{baseUrl}/confirm-email?userId={user.Id}&code={encodedToken}";
            var emailBody = $"Por favor, confirme su correo electrónico <a href='{callbackUrl}'>haciendo clic aquí</a>.";

            await _emailSender.SendEmailAsync(user.Email!, "Confirmar correo electrónico", emailBody);
        }
    }
}
