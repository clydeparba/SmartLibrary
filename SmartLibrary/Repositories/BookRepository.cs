using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext ctx) : base(ctx) { }

        public async Task<Book?> GetByISBNAsync(string isbn) =>
            await _db.FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task<IEnumerable<Book>> SearchAsync(string keyword)
        {
            return await _db
                .Where(b =>
                    b.Title.Contains(keyword) ||
                    b.Author.Contains(keyword) ||
                    b.ISBN.Contains(keyword))
                .ToListAsync();
        }
    }
}
