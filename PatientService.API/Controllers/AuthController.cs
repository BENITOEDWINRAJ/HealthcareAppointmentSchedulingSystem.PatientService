using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Application.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PatientService.Application.Handlers.Interfaces;
using PatientService.Application.Commands;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IJwtService _jwt;
        private readonly ILogger<AuthController> _logger;
        private readonly IRegisterUserHandler _registerHandler;
        private readonly ILoginHandler _loginHandler;

        public AuthController(IUserRepository repo, IJwtService jwt, ILogger<AuthController> logger, IRegisterUserHandler registerHandler, ILoginHandler loginHandler)
        {
            _repo = repo;
            _jwt = jwt;
            _logger = logger;
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };
            _logger.LogInformation("User registration started for {Username}",
        request.Username);
            //var id = await _createHandler.Handle(command);
            //await _repo.AddAsync(user);
            var id =await _registerHandler.Handle(new Application.Commands.RegisterUserCommand
            {
                Username = request.Username,
                Password = request.Password,
                Role = request.Role
            });

            _logger.LogInformation("User registered successfully {Username}",
        request.Username);

            return Ok(user.Id);
        }

        

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Username}", request.Username);

            try
            {
                var query = new LoginCommand(request.Username, request.Password);
                var token = await _loginHandler.Handle(query);

                return Ok(new { Token = token });                
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid username or password");
            }
        }       

        
    }
}
