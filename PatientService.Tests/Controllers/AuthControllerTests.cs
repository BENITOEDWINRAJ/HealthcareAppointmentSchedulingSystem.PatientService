using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PatientService.API.Controllers;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Application.DTOs;
using FluentAssertions;
using PatientService.Application.Handlers.Interfaces;
using PatientService.Application.Handlers;
using PatientService.Application.Commands;


namespace PatientService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Mock<IRegisterUserHandler> _registerHandler;
        private readonly Mock<ILoginHandler> _loginHandler;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _jwtMock = new Mock<IJwtService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _registerHandler = new Mock<IRegisterUserHandler>();
            _loginHandler = new Mock<ILoginHandler>();
            _controller = new AuthController(
                _repoMock.Object,
                _jwtMock.Object,
                _loggerMock.Object,
                _registerHandler.Object,
                _loginHandler.Object
            );
        }

        [Fact]
        public async Task Register_ShouldCreateUser_ReturnUserId()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "testuser",
                Password = "password123",
                Role = "Admin"
            };

            var userId = Guid.NewGuid();

            _registerHandler
                .Setup(x => x.Handle(It.IsAny<RegisterUserCommand>()))
                .ReturnsAsync(userId);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<Guid>();

            _registerHandler.Verify(
                x => x.Handle(It.IsAny<RegisterUserCommand>()),
                Times.Once);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            /* var password = "password123";
             var hashed = BCrypt.Net.BCrypt.HashPassword(password);

             var user = new User
             {
                 Id = Guid.NewGuid(),
                 Username = "testuser",
                 PasswordHash = hashed,
                 Role = "Doctor"
             };

             var request = new LoginRequest
             {
                 Username = "testuser",
                 Password = password
             };

             _repoMock
                 .Setup(x => x.GetByUsernameAsync(request.Username))
                 .ReturnsAsync(user);

             _jwtMock
                 .Setup(x => x.GenerateToken(user))
                 .Returns("fake-jwt-token");

             // Act
             var result = await _controller.Login(request);

             // Assert
             var ok = result as OkObjectResult;

             ok.Should().NotBeNull();
             ok.Value.Should().Be("fake-jwt-token");*/
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "password"
            };

            _loginHandler
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ReturnsAsync("fake-jwt-token");

            var result = await _controller.Login(request);

            var ok = result as OkObjectResult;

            ok.Should().NotBeNull();

            var value = ok!.Value;

            var tokenProperty = value!.GetType().GetProperty("Token");
            var tokenValue = tokenProperty!.GetValue(value)?.ToString();

            tokenValue.Should().Be("fake-jwt-token");
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordInvalid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "wrong-password"
            };

            _loginHandler
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "unknown",
                Password = "password"
            };

            _loginHandler
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Register_ReturnsOk_WithUserId()
        {
            var request = new RegisterRequest
            {
                Username = "testuser",
                Password = "password",
                Role = "Patient"
            };

            var userId = Guid.NewGuid();

            _registerHandler
                .Setup(x => x.Handle(It.IsAny<RegisterUserCommand>()))
                .ReturnsAsync(userId);

            var result = await _controller.Register(request);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }
        
        // ----------------------------------------------------
        // Login - Unauthorized
        // ----------------------------------------------------
        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
        {
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            _loginHandler
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            var result = await _controller.Login(request);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

    }
}