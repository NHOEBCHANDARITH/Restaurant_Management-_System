using Restaurant_Management__System.Models;


namespace Restaurant_Management__System.Repositories
{
    public interface IEntityBaseRepository<T>
        where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<bool> ExistsAsync(int id);

        Task SaveAsync();
    }
}