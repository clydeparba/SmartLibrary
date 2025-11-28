using SmartLibrary.Models;

namespace SmartLibrary.Interfaces
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetActiveLoansByUserAsync(int userId);
    }
}
