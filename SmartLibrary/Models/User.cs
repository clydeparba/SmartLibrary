using SmartLibrary.Models;

namespace SmartLibrary.Models
{
    public abstract class User
    {   
        public int Id { get; set; }

        public required string Username { get; set; }
        public required string Password { get; set; }

        public required string Name { get; set; }
        public required string Email { get; set; }

        // Role: "Student" or "Faculty" or "Staff"
        public required string Role { get; set; }

        // Borrowing rules
        public int BorrowLimit { get; set; }
        public int LoanDays { get; set; }

        // Navigation
        public List<Loan> Loans { get; set; } = new();
    }
}
