using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryContext ctx) : base(ctx) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
