using PatientService.Application.Handlers.Interfaces;
using PatientService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PatientService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PatientService.Application.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        private readonly IUserRepository _repo;
        private readonly IJwtService _jwtService;                
        private readonly IRegisterUserHandler _registerHandler;

        public LoginHandler(IUserRepository repo, IJwtService jwtService, IRegisterUserHandler registerUserHandler)
        {
            _repo = repo;
            _jwtService = jwtService;            
            _registerHandler = registerUserHandler;
        }

        public async Task<string> Handle(LoginCommand query)
        {
            
            var user = await _repo.GetByUsernameAsync(query.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(query.Password, user.PasswordHash))
                return "Please check your Credentials";            

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            return token;
        }
    }
}
