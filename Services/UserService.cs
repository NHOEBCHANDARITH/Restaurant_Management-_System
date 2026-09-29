//using Restaurant_Management__System.Models;
//using Restaurant_Management__System.Repositories;

//namespace Restaurant_Management__System.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IEntityBaseRepository<User> _repository;

//        public UserService(
//            IEntityBaseRepository<User> repository)
//        {
//            _repository = repository;
//        }

//        public async Task<IEnumerable<User>> GetAllAsync()
//        {
//            return await _repository.GetAllAsync();
//        }

//        public async Task<User?> GetByIdAsync(int id)
//        {
//            return await _repository.GetByIdAsync(id);
//        }

//        public async Task CreateAsync(User user)
//        {
//            user.CreatedAt = DateTime.Now;

//            await _repository.AddAsync(user);

//            await _repository.SaveAsync();
//        }

//        public async Task UpdateAsync(User user)
//        {
//            user.UpdatedAt = DateTime.Now;

//            _repository.Update(user);

//            await _repository.SaveAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var user = await _repository.GetByIdAsync(id);

//            if (user == null)
//                return;

//            _repository.Delete(user);

//            await _repository.SaveAsync();
//        }
//    }
//}
