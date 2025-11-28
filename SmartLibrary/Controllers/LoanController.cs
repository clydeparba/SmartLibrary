using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Interfaces;
using SmartLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loans;
        private readonly LibraryContext _ctx;

        public LoanController(ILoanService loans, LibraryContext ctx)
        {
            _loans = loans;
            _ctx = ctx;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromQuery] int userId, [FromQuery] int bookId)
        {
            var loan = await _loans.BorrowAsync(userId, bookId);
            return Ok(loan);
        }

        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> Return(int loanId)
        {
            var loan = await _loans.ReturnAsync(loanId);
            return Ok(loan);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var loans = await _ctx.Loans
                .Include(l => l.Book)
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return Ok(loans);
        }

        [HttpGet("{loanId}/fine")]
        public async Task<IActionResult> GetFine(int loanId)
        {
            return Ok(await _loans.CalculateFineAsync(loanId));
        }
    }
}
