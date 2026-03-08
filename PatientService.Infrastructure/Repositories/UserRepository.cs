using Microsoft.EntityFrameworkCore;
using PatientService.Core.Entities;
using PatientService.Core.Repositories;
using PatientService.Infrastructure.Data;
using PatientService.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKafka.Events;

namespace PatientService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly KafkaProducerService _kafka;        

        public UserRepository(ApplicationDbContext context, KafkaProducerService kafka)
        {
            _context = context;
            _kafka = kafka;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _kafka.PublishUserCreated(new UserCreatedEvent
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role
            });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                throw new InvalidOperationException($"User with username '{username}' not found.");
            }
            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
