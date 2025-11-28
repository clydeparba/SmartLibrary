using SmartLibrary.Models;

namespace SmartLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }

        public int TotalCopies { get; set; }
        public int CopiesAvailable { get; set; } = 0;

        // Navigation
        public List<Loan> Loans { get; set; } = new();
    }
}
