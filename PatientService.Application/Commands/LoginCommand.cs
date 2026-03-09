using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Application.Commands
{
    public class LoginCommand
    {
        public string Username { get; }
        public string Password { get; }

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
