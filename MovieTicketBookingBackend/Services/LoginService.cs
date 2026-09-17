using MovieTicketBooking.Data;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public class LoginService
    {
        private readonly LoginDataStore loginDataStore;

        public LoginService(LoginDataStore loginDataStore)
        {
            this.loginDataStore = loginDataStore;
        }

        public LoginDetails? Login()
        {
            Console.Write("Login ID: ");
            string loginID = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            LoginDetails? login =
                loginDataStore.ValidateLogin(
                    loginID,
                    password
                );

            if (login == null)
            {
                Console.WriteLine("Invalid Login ID or Password.");
                return null;
            }

            Console.WriteLine("Login successful!");

            return login;
        }
    }
}