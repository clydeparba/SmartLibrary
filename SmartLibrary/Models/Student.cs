namespace SmartLibrary.Models
{
    public class Student : User
    {
        public string? Program { get; set; }

        public Student()
        {
            Role = "Student";
            BorrowLimit = 3;
            LoanDays = 3;
        }
    }
}
