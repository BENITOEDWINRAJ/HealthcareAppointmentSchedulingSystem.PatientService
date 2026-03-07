using PatientService.Application.Commands;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;

using PatientService.Core.Entities;
using PatientService.Core.Repositories;

namespace PatientService.Application.Handlers
{
    public class RegisterUserHandler
    {
        private readonly IUserRepository _repository;

        public RegisterUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(RegisterUserCommand command)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                Role = command.Role
            };

            await _repository.AddAsync(user);

            return user.Id;
        }
    }
}
