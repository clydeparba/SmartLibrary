using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Book>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<Book?> GetAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<Book?> GetByISBNAsync(string isbn) =>
            await _repo.GetByISBNAsync(isbn);

        public async Task<IEnumerable<Book>> SearchAsync(string keyword) =>
            await _repo.SearchAsync(keyword);

        public async Task<Book> AddAsync(Book book)
        {
            await _repo.AddAsync(book);
            await _repo.SaveAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _repo.Update(book);
            await _repo.SaveAsync();
            return book;
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book != null)
            {
                _repo.Remove(book);
                await _repo.SaveAsync();
            }
        }
    }
}
