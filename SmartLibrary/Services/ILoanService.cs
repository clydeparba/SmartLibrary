using SmartLibrary.Models;

namespace SmartLibrary.Interfaces
{
    public interface ILoanService
    {
        Task<Loan> BorrowAsync(int userId, int bookId);
        Task<Loan> ReturnAsync(int loanId);
        Task<decimal> CalculateFineAsync(int loanId);
    }
}
