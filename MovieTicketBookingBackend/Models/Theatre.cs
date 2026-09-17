namespace MovieTicketBooking.Models
{
    public class Theatre
    {
        public List<Show> Shows { get; set; } =
            new List<Show>();

        public int TheatreID { get; set; }

        public string TheatreName { get; set; }

        public int NumberofSeats { get; set; }

        public int PlatinumSeats { get; set; }

        public int GoldSeats { get; set; }

        public int SilverSeats { get; set; }

        public Theatre()
        {
        }

        public Theatre(
            string theatreName,
            int numberOfSeats,
            int platinumSeats,
            int goldSeats,
            int silverSeats)
        {
            Random random = new Random();

            TheatreID = random.Next(1000, 9999);

            TheatreName = theatreName;
            NumberofSeats = numberOfSeats;

            PlatinumSeats = platinumSeats;
            GoldSeats = goldSeats;
            SilverSeats = silverSeats;
        }

        public void DisplayTheatreDetails()
        {
            Console.WriteLine($"Theatre ID: {TheatreID}");
            Console.WriteLine($"Theatre Name: {TheatreName}");
            Console.WriteLine($"Total Seats: {NumberofSeats}");
            Console.WriteLine($"Platinum Seats: {PlatinumSeats}");
            Console.WriteLine($"Gold Seats: {GoldSeats}");
            Console.WriteLine($"Silver Seats: {SilverSeats}");
        }
    }
}