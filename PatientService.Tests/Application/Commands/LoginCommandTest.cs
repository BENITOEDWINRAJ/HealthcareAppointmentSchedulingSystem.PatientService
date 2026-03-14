using PatientService.Application.Commands;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Tests.Application.Commands
{
    public class LoginCommandTest
    {
        [Fact]
        public void Constructor_Should_Set_Username_And_Password()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";

            // Act
            var command = new LoginCommand(username, password);

            // Assert
            Assert.Equal(username, command.Username);
            Assert.Equal(password, command.Password);
        }

        [Fact]
        public void Constructor_Should_Not_Return_Null_Values()
        {
            // Arrange
            var username = "user1";
            var password = "password1";

            // Act
            var command = new LoginCommand(username, password);

            // Assert
            Assert.NotNull(command.Username);
            Assert.NotNull(command.Password);
        }
    }
}
