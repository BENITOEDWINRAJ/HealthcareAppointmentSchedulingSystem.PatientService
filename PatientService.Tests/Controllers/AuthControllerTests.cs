using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PatientService.API.Controllers;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Application.DTOs;
using FluentAssertions;


namespace PatientService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;

        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _jwtMock = new Mock<IJwtService>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _repoMock.Object,
                _jwtMock.Object,
                _loggerMock.Object
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

            _repoMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            _repoMock.Verify(x =>
                x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var password = "password123";
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
            ok.Value.Should().Be("fake-jwt-token");
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordInvalid()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct-password"),
                Role = "Patient"
            };

            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "wrong-password"
            };

            _repoMock
                .Setup(x => x.GetByUsernameAsync(request.Username))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
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

            _repoMock
                .Setup(x => x.GetByUsernameAsync(request.Username))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}