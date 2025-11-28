namespace SmartLibrary.Models
{
    public class Staff : User
    {
        public Staff()
        {
            Role = "Staff";
            BorrowLimit = 0;
            LoanDays = 0;
        }
    }
}
