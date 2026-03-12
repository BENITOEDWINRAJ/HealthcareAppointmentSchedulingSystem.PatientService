using PatientService.Application.Commands;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;

using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Application.Handlers.Interfaces;

namespace PatientService.Application.Handlers
{
    public class RegisterUserHandler :IRegisterUserHandler
    {
        private readonly IUserRepository _repository;

        public RegisterUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

       /*  public async Task<Guid> Handle(RegisterUserCommand command)
         {
             var user = new User
             {
                 Id = Guid.NewGuid(),
                 Username = command.Username,
                 Password = command.Password,
                 PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                 Role = command.Role
             };

             await _repository.AddAsync(user);

             return user.Id;
         }*/
        public async Task<Guid> Handle(RegisterUserCommand command)
        {
            var existingUser = await _repository.GetByUsernameAsync(command.Username);

            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Password = command.Password,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                Role = command.Role
            };
            
            await _repository.AddAsync(user);        
            
            return user.Id;
            
        }
    }
}
