using SmartLibrary.Data;
using SmartLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.Services
{
    public class FineService
    {
        private readonly LibraryContext _ctx;

        public FineService(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Fine>> GetAllAsync()
        {
            return await _ctx.Fines
                .Include(f => f.Loan)
                .ThenInclude(l => l.User)
                .Include(f => f.Loan.Book)
                .ToListAsync();
        }
    }
}
