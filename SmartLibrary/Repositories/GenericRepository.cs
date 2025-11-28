using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;

namespace SmartLibrary.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryContext _ctx;
        protected readonly DbSet<T> _db;

        public GenericRepository(LibraryContext ctx)
        {
            _ctx = ctx;
            _db = _ctx.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _db.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _db.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
