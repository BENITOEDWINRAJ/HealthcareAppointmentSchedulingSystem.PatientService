using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PatientService.API.Controllers;
using PatientService.Application.Handlers.Interfaces;
using PatientService.Application.Commands;
using PatientService.Application.DTOs;
using PatientService.Core.Repositories;
using System;
using System.Threading.Tasks;


namespace PatientService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _repoMock = new();
        private readonly Mock<IJwtService> _jwtMock = new();
        private readonly Mock<ILogger<AuthController>> _loggerMock = new();
        private readonly Mock<IRegisterUserHandler> _registerHandlerMock = new();
        private readonly Mock<ILoginHandler> _loginHandlerMock = new();
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController(
            _repoMock.Object,
            _jwtMock.Object,
            _loggerMock.Object,
            _registerHandlerMock.Object,
            _loginHandlerMock.Object);
        }        

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange            
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "password"
               // ,Role = "Patient"
            };

            _loginHandlerMock
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
                //,Role = "Doctor"
            };

            _loginHandlerMock
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
                //,Role = "Patient"
            };

            _loginHandlerMock
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }        
        
        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
        {
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "wrongpassword",
               //, Role = "Doctor"
            };

            _loginHandlerMock
                .Setup(x => x.Handle(It.IsAny<LoginCommand>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            var result = await _controller.Login(request);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "john",
                Password = "123456",
                Role = "Patient"
            };

            var expectedUserId = Guid.NewGuid();

            _registerHandlerMock
                .Setup(x => x.Handle(It.IsAny<RegisterUserCommand>()))
                .ReturnsAsync(expectedUserId);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }        

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "john",
                Password = "123456",
                Role = "Patient"
            };

            _registerHandlerMock
                .Setup(x => x.Handle(It.IsAny<RegisterUserCommand>()))
                .ThrowsAsync(new Exception("User already exists"));

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }       
               
    }
}