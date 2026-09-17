namespace MovieTicketBooking.Models
{
    public class Show
    {
        public List<Booking> Bookings { get; set; } =
    new List<Booking>();

        public int ShowID { get; set; }
        public string MovieID { get; set; }
        public int TheatreID { get; set; }
        public Movie Movie { get; set; }
        public Theatre Theatre { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PlatinumSeatRate { get; set; }
        public decimal GoldSeatRate { get; set; }
        public decimal SilverSeatRate { get; set; }

        public Show()
        {

        }
        public Show(
            string movieID,
            int theatreID,
            DateTime startDate,
            DateTime endDate,
            decimal platinumSeatRate,
            decimal goldSeatRate,
            decimal silverSeatRate)
        {
            Random random = new Random();

            ShowID = random.Next(1000, 9999);

            MovieID = movieID;
            TheatreID = theatreID;
            StartDate = startDate;
            EndDate = endDate;
            PlatinumSeatRate = platinumSeatRate;
            GoldSeatRate = goldSeatRate;
            SilverSeatRate = silverSeatRate;
        }

        public void DisplayShowDetails()
        {
            Console.WriteLine($"Show ID: {ShowID}");
            Console.WriteLine($"Movie ID: {MovieID}");
            Console.WriteLine($"Theatre ID: {TheatreID}");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"End Date: {EndDate}");
            Console.WriteLine($"Platinum Seat Rate: {PlatinumSeatRate}");
            Console.WriteLine($"Gold Seat Rate: {GoldSeatRate}");
            Console.WriteLine($"Silver Seat Rate: {SilverSeatRate}");
        }
    }
}