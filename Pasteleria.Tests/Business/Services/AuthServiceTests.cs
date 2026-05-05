using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Moq;
using Pasteleria.Business.Configuration;
using Pasteleria.Business.Interfaces.Repositories;
using Pasteleria.Business.Services;
using Pasteleria.Shared.Auth.Dtos;
using Pasteleria.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;

namespace Pasteleria.Tests.Business.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOptions<JwtConfig>> _jwtConfigMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _mapperMock = new Mock<IMapper>();
            _jwtConfigMock = new Mock<IOptions<JwtConfig>>();
            _emailSenderMock = new Mock<IEmailSender>();

            _jwtConfigMock.Setup(x => x.Value).Returns(new JwtConfig { Secret = "super_secret_key_for_testing_purposes_only" });

            _authService = new AuthService(
                _authRepositoryMock.Object,
                _mapperMock.Object,
                _jwtConfigMock.Object,
                _emailSenderMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_EmailExists_ReturnsFailure()
        {
            // Arrange
            var request = new UserRegistrationRequestDto { Email = "test@test.com" };
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new User());

            // Act
            var result = await _authService.RegisterAsync(request, "http://localhost");

            // Assert
            result.Result.Should().BeFalse();
            result.Errors.Should().Contain("Ya existe este correo.");
        }

        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var request = new UserRegistrationRequestDto { Email = "new@test.com", Password = "Password123!" };
            var newUser = new User { Email = request.Email };
            
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _mapperMock.Setup(x => x.Map<User>(request)).Returns(newUser);
            _authRepositoryMock.Setup(x => x.CreateUserAsync(newUser, request.Password)).ReturnsAsync(IdentityResult.Success);
            _authRepositoryMock.Setup(x => x.AddToRoleAsync(newUser, "Visitor")).ReturnsAsync(IdentityResult.Success);
            _authRepositoryMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(newUser)).ReturnsAsync("token");
            _emailSenderMock.Setup(x => x.SendEmailAsync(newUser.Email, It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(request, "http://localhost");

            // Assert
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var dto = new UserLoginRequestDto { Email = "test@test.com", Password = "WrongPassword" };
            var user = new User { Email = dto.Email };
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _authRepositoryMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(false);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Result.Should().BeFalse();
            result.Errors.Should().Contain("Credenciales inválidas.");
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessWithToken()
        {
            // Arrange
            var dto = new UserLoginRequestDto { Email = "test@test.com", Password = "Password123!" };
            var user = new User { Id = "1", Email = dto.Email, EmailConfirmed = true };
            
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _authRepositoryMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);
            _authRepositoryMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Result.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();
        }
    }
}
