using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<User?> GetAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<User> AddAsync(User user)
        {
            await _repo.AddAsync(user);
            await _repo.SaveAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _repo.Update(user);
            await _repo.SaveAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null)
            {
                _repo.Remove(user);
                await _repo.SaveAsync();
            }
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _repo.GetByUsernameAsync(username);
            if (user == null || user.Password != password)
                return null;

            return user;
        }
    }
}
