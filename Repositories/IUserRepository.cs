using E_ranga.Models;

namespace Namespace
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserRegister user);
        Task<UserRegister> GetUserByEmailAsync(string email);

        Task<IEnumerable<UserRegister>> GetAllUsersAsync();
        Task<UserRegister> GetUserByIdAsync(int id);
        Task UpdateUserAsync(UserRegister user);
        Task DeleteUserAsync(UserRegister user);
    }
}