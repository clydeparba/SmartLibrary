using SmartLibrary.Models;

public class Loan
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public required User User { get; set; }

    public int BookId { get; set; }
    public required Book Book { get; set; }

    public DateTime BorrowedAt { get; set; }
    public DateTime DueAt { get; set; }

    public bool Returned { get; set; } = false;
    public DateTime? ReturnedAt { get; set; }

    public Fine? Fine { get; set; }

    // 🔥 EF Core needs a parameterless constructor to materialize objects
    public Loan() { }

    // ✔ Your custom constructor (keep it, but EF won’t use it)
    public Loan(User user, Book book)
    {
        User = user;
        UserId = user.Id;

        Book = book;
        BookId = book.Id;

        BorrowedAt = DateTime.Now;
        DueAt = DateTime.Now.AddDays(14);
    }
}
