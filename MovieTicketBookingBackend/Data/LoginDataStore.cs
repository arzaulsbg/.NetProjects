using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class LoginDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public LoginDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddLogin(LoginDetails login)
        {
            LoginDetails? existingLogin =
                GetLoginById(login.LoginID);

            if (existingLogin != null)
            {
                return false;
            }

            context.LoginDetails.Add(login);
            context.SaveChanges();

            return true;
        }

        // READ
        public LoginDetails? GetLoginById(string loginID)
        {
            return context.LoginDetails
                .FirstOrDefault(l => l.LoginID == loginID);
        }

        // LOGIN VALIDATION
        public LoginDetails? ValidateLogin(
            string loginID,
            string password)
        {
            return context.LoginDetails
                .FirstOrDefault(l =>
                    l.LoginID == loginID &&
                    l.Password == password);
        }

        // DISPLAY
        public void DisplayAllLogins()
        {
            List<LoginDetails> logins =
                context.LoginDetails.ToList();

            foreach (LoginDetails login in logins)
            {
                Console.WriteLine(
                    $"Login ID: {login.LoginID}"
                );

                Console.WriteLine(
                    $"Login Type: {login.LoginType}"
                );

                Console.WriteLine("------------------------");
            }
        }
    }
}