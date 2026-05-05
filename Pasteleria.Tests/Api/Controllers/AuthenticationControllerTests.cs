using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pasteleria.Api.Controllers;
using Pasteleria.Business.Interfaces.Services;
using Pasteleria.Shared.Auth.Auxiliar;
using Pasteleria.Shared.Auth.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pasteleria.Tests.Api.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthenticationController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var dto = new UserLoginRequestDto { Email = "test@test.com", Password = "Password123!" };
            var authResult = new AuthResult { Result = true, Token = "token_string" };
            _authServiceMock.Setup(x => x.LoginAsync(dto)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var dto = new UserLoginRequestDto { Email = "test@test.com", Password = "WrongPassword" };
            var authResult = new AuthResult { Result = false, Errors = new List<string> { "Credenciales inválidas." } };
            _authServiceMock.Setup(x => x.LoginAsync(dto)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new UserRegistrationRequestDto { Email = "new@test.com", Password = "Password123!" };
            var authResult = new AuthResult { Result = true };
            _authServiceMock.Setup(x => x.RegisterAsync(request, It.IsAny<string>())).ReturnsAsync(authResult);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }
    }
}
