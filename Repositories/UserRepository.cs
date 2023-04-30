using E_ranga.Data;
using E_ranga.Models;
using Namespace;
using Microsoft.EntityFrameworkCore;

namespace Namespace2
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUserAsync(UserRegister user)
        {
            await _dbContext.UserRegister.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<UserRegister> GetUserByEmailAsync(string email)
        {
            return await _dbContext.UserRegister.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}