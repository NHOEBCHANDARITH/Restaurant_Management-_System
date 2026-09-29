using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;
using Microsoft.EntityFrameworkCore;
namespace Restaurant_Management__System.Repositories
{
    public class EntityBaseRepository<T> :
        IEntityBaseRepository<T>
        where T : EntityBase
    {
        private readonly ApplicationDbContext _context;

        public EntityBaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;

            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<T>()
                .AnyAsync(x => x.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
