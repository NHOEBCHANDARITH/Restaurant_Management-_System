using Restaurant_Management__System.Models;

namespace Restaurant_Management__System.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);
    }
}
