using E_ranga.Models;

namespace Namespace
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserRegister user);
        Task<UserRegister> GetUserByEmailAsync(string email);
    }
}