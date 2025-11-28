using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Services
{
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _ctx;

        public LoanService(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Loan> BorrowAsync(int userId, int bookId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            var book = await _ctx.Books.FindAsync(bookId);

            if (user == null)
                throw new ArgumentException("User not found");
            if (book == null || book.CopiesAvailable <= 0)
                throw new InvalidOperationException("Book not available");

            var activeLoans = await _ctx.Loans
                .CountAsync(l => l.UserId == userId && !l.Returned);

            if (activeLoans >= user.BorrowLimit)
                throw new InvalidOperationException("Borrow limit reached");

            // Fixed: Set required members User and Book in object initializer
            var loan = new Loan(user, book)
            {
                User = user,
                Book = book,
                BorrowedAt = DateTime.UtcNow,
                DueAt = DateTime.UtcNow.AddDays(user.LoanDays)
            };

            book.CopiesAvailable--;
            _ctx.Loans.Add(loan);
            _ctx.Books.Update(book);
            await _ctx.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan> ReturnAsync(int loanId)
        {
            var loan = await _ctx.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new ArgumentException("Loan not found");
            if (loan.Returned)
                throw new InvalidOperationException("Already returned");

            loan.Returned = true;
            loan.ReturnedAt = DateTime.UtcNow;
            loan.Book.CopiesAvailable++;

            var fineAmount = await CalculateFineAsync(loanId);
            if (fineAmount > 0)
            {
                // Fixed: Set required member Loan in object initializer
                var fine = new Fine(loan, fineAmount)
                {
                    Loan = loan,
                    AssessedAt = DateTime.UtcNow
                };
                _ctx.Fines.Add(fine);
            }

            _ctx.Loans.Update(loan);
            _ctx.Books.Update(loan.Book);
            await _ctx.SaveChangesAsync();

            return loan;
        }

        public async Task<decimal> CalculateFineAsync(int loanId)
        {
            var loan = await _ctx.Loans
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new ArgumentException("Loan not found");

            var today = loan.ReturnedAt ?? DateTime.UtcNow;
            int overdueDays = (today.Date - loan.DueAt.Date).Days;

            if (overdueDays <= 0)
                return 0;

            decimal rate = loan.User switch
            {
                Student => 1.00m,
                Faculty => 0.50m,
                _ => 1.00m
            };

            return overdueDays * rate;
        }
    }
}