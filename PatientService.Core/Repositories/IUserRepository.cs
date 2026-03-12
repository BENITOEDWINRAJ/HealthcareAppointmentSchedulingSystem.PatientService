using PatientService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetByUsernameAsync(string username);
        public Task<User> GetByIdAsync(Guid id);
        public Task<List<User>> GetAllAsync();
        public Task AddAsync(User user); 
    }
}
