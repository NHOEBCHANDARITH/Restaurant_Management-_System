//using Restaurant_Management__System.Models;
//using Restaurant_Management__System.Repositories;
//using Restaurant_Management__System.Services;
//namespace Restaurant_Management__System.Services
//{
//    public interface CategoryService : ICategoryService
//    {
//        private readonly IEntityBaseRepository<Category> _repository;

//        public CategoryService(
//            IEntityBaseRepository<Category> repository)
//        {
//            _repository = repository;
//        }

//        public async Task<IEnumerable<Category>> GetAllAsync()
//        {
//            return await _repository.GetAllAsync();
//        }

//        public async Task<Category?> GetByIdAsync(int id)
//        {
//            return await _repository.GetByIdAsync(id);
//        }

//        public async Task CreateAsync(Category category)
//        {
//            await _repository.AddAsync(category);
//            await _repository.SaveAsync();
//        }

//        public async Task UpdateAsync(Category category)
//        {
//            _repository.Update(category);
//            await _repository.SaveAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var category = await _repository.GetByIdAsync(id);

//            if (category == null)
//                return;

//            _repository.Delete(category);
//            await _repository.SaveAsync();
//        }
//    }
//}
