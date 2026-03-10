using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        
        public  string PasswordHash { get; set; }
        
        public required string Role { get; set; }
        
        public required string Password { get; set; }
    }
}
