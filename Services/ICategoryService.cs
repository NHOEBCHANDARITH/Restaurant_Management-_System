using Restaurant_Management__System.Models;

namespace Restaurant_Management__System.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task CreateAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(int id);
    }
}
