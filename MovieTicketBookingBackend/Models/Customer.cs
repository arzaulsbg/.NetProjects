namespace MovieTicketBooking.Models
{
    public class Customer
    {
        public List<Booking> Bookings { get; set; } =
            new List<Booking>();

        public int CustomerID { get; set; }

        public string LoginID { get; set; }

        public string CustomerName { get; set; }

        public string City { get; set; }

        public Customer()
        {
        }

        public Customer(
            string loginID,
            string customerName,
            string city)
        {
            Random random = new Random();

            CustomerID = random.Next(1000, 9999);

            LoginID = loginID;
            CustomerName = customerName;
            City = city;
        }

        public void DisplayCustomerDetails()
        {
            Console.WriteLine($"Customer ID: {CustomerID}");
            Console.WriteLine($"Login ID: {LoginID}");
            Console.WriteLine($"Customer Name: {CustomerName}");
            Console.WriteLine($"City: {City}");
        }
    }
}
