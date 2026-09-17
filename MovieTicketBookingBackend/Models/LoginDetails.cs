namespace MovieTicketBooking.Models
{
    public class LoginDetails
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
        public LoginDetails()
        {
        }
        public LoginDetails(string loginID, string loginType)
        {
            LoginID = loginID;
            LoginType = loginType;
            Password = LoginID;
        }

        public void DisplayLoginDetails()
        {
            Console.WriteLine($"Login ID: {LoginID}");
            Console.WriteLine($"Password: {Password}");
            Console.WriteLine($"Login Type: {LoginType}");
        }
    }
}