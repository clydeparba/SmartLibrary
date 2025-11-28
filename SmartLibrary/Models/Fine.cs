public class Fine
{
    public int Id { get; set; }

    public int LoanId { get; set; }
    public required Loan Loan { get; set; }

    public decimal Amount { get; set; }
    public DateTime AssessedAt { get; set; }

    // EF Core requires a parameterless constructor
    private Fine() { }

    // Your domain constructor
    public Fine(Loan loan, decimal amount)
    {
        Loan = loan;
        LoanId = loan.Id;
        Amount = amount;
        AssessedAt = DateTime.Now;
    }
}
