using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class TheatreDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public TheatreDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddTheatre(Theatre theatre)
        {
            Theatre? existingTheatre =
                GetTheatreById(theatre.TheatreID);

            if (existingTheatre != null)
            {
                return false;
            }

            context.Theatres.Add(theatre);
            context.SaveChanges();

            return true;
        }

        // READ - All theatres
        public List<Theatre> GetAllTheatres()
        {
            return context.Theatres.ToList();
        }

        // READ - By ID
        public Theatre? GetTheatreById(int theatreID)
        {
            return context.Theatres
                .FirstOrDefault(t => t.TheatreID == theatreID);
        }

        // UPDATE
        public bool UpdateTheatre(
            int theatreID,
            string newTheatreName,
            int newNumberOfSeats)
        {
            Theatre? theatre =
                GetTheatreById(theatreID);

            if (theatre == null)
            {
                return false;
            }

            theatre.TheatreName = newTheatreName;
            theatre.NumberofSeats = newNumberOfSeats;

            context.SaveChanges();

            return true;
        }

        // DELETE
        public bool DeleteTheatre(int theatreID)
        {
            Theatre? theatre =
                GetTheatreById(theatreID);

            if (theatre == null)
            {
                return false;
            }

            context.Theatres.Remove(theatre);
            context.SaveChanges();

            return true;
        }

        // DISPLAY
        public void DisplayAllTheatres()
        {
            List<Theatre> theatres = GetAllTheatres();

            foreach (Theatre theatre in theatres)
            {
                theatre.DisplayTheatreDetails();
                Console.WriteLine("------------------------");
            }
        }
    }
}