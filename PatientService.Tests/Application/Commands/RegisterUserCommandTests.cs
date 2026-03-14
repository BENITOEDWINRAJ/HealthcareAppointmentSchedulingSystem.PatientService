using PatientService.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Tests.Application.Commands
{
    public class RegisterUserCommandTests
    {
        [Fact]
        public void Properties_Should_Set_And_Get_Correctly()
        {
            var command = new RegisterUserCommand
            {
                Username = "testuser",
                Password = "testpass",
                Role = "Admin"
            };

            Assert.Equal("testuser", command.Username);
            Assert.Equal("testpass", command.Password);
            Assert.Equal("Admin", command.Role);
        }
    }
}
