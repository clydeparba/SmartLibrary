namespace SmartLibrary.Models
{
    public class Faculty : User
    {
        public string? Department { get; set; }

        public Faculty()
        {
            Role = "Faculty";
            BorrowLimit = 5;
            LoanDays = 7;
        }
    }
}
