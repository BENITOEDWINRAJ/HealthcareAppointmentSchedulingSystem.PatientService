using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Application.DTOs;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwt;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository repo, JwtService jwt, ILogger<AuthController> logger)
        {
            _repo = repo;
            _jwt = jwt;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };
            _logger.LogInformation("User registration started for {Username}",
        request.Username);

            await _repo.AddAsync(user);

            _logger.LogInformation("User registered successfully {Username}",
        request.Username);

            return Ok(user.Id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Username}",
        request.Username);
            var user = await _repo.GetByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized();

            var token = _jwt.GenerateToken(user);

            _logger.LogInformation("Login successful {Username}",
        request.Username);

            return Ok(token);
        }
    }
}
