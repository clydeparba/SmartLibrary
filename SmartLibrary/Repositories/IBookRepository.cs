using SmartLibrary.Models;

namespace SmartLibrary.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> SearchAsync(string keyword);
    }
}
