namespace MovieTicketBooking.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int ShowID { get; set; }
        public Customer Customer { get; set; }

        public Show Show { get; set; }

        public int PlatinumSeats { get; set; }
        public int GoldSeats { get; set; }
        public int SilverSeats { get; set; }

        public decimal TotalAmount { get; set; }
        public Booking()
        {

        }
        public Booking(
            int customerID,
            int showID,
            int platinumSeats,
            int goldSeats,
            int silverSeats,
            decimal platinumRate,
            decimal goldRate,
            decimal silverRate)
        {
            Random random = new Random();

            BookingID = random.Next(1000, 9999);

            CustomerID = customerID;
            ShowID = showID;

            PlatinumSeats = platinumSeats;
            GoldSeats = goldSeats;
            SilverSeats = silverSeats;

            TotalAmount =
                (PlatinumSeats * platinumRate) +
                (GoldSeats * goldRate) +
                (SilverSeats * silverRate);
        }

        public void DisplayBookingDetails()
        {
            Console.WriteLine($"Booking ID: {BookingID}");
            Console.WriteLine($"Customer ID: {CustomerID}");
            Console.WriteLine($"Show ID: {ShowID}");
            Console.WriteLine($"Platinum Seats: {PlatinumSeats}");
            Console.WriteLine($"Gold Seats: {GoldSeats}");
            Console.WriteLine($"Silver Seats: {SilverSeats}");
            Console.WriteLine($"Total Amount: {TotalAmount}");
        }
    }
}