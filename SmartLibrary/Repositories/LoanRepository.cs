using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Repositories
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        public LoanRepository(LibraryContext ctx) : base(ctx) { }

        public async Task<IEnumerable<Loan>> GetActiveLoansByUserAsync(int userId)
        {
            return await _db
                .Where(l => l.UserId == userId && !l.Returned)
                .Include(l => l.Book)
                .ToListAsync();
        }
    }
}
