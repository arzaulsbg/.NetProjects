using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ShowDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public ShowDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddShow(Show show)
        {
            Show? existingShow = GetShowById(show.ShowID);

            if (existingShow != null)
            {
                return false;
            }

            context.Shows.Add(show);
            context.SaveChanges();

            return true;
        }

        // READ - All shows
        public List<Show> GetAllShows()
        {
            return context.Shows.ToList();
        }

        // READ - By ID
        public Show? GetShowById(int showID)
        {
            return context.Shows
                .FirstOrDefault(s => s.ShowID == showID);
        }

        // READ - Shows for a movie
        public List<Show> GetShowsByMovie(string movieID)
        {
            return context.Shows
                .Where(s => s.MovieID == movieID)
                .ToList();
        }

        // UPDATE
        public bool UpdateShow(
            int showID,
            DateTime newStartDate,
            DateTime newEndDate,
            decimal newPlatinumRate,
            decimal newGoldRate,
            decimal newSilverRate)
        {
            Show? show = GetShowById(showID);

            if (show == null)
            {
                return false;
            }

            show.StartDate = newStartDate;
            show.EndDate = newEndDate;
            show.PlatinumSeatRate = newPlatinumRate;
            show.GoldSeatRate = newGoldRate;
            show.SilverSeatRate = newSilverRate;

            context.SaveChanges();

            return true;
        }

        // DELETE
        public bool DeleteShow(int showID)
        {
            Show? show = GetShowById(showID);

            if (show == null)
            {
                return false;
            }

            context.Shows.Remove(show);
            context.SaveChanges();

            return true;
        }

        // DISPLAY
        public void DisplayAllShows()
        {
            List<Show> shows = GetAllShows();

            foreach (Show show in shows)
            {
                show.DisplayShowDetails();
                Console.WriteLine("------------------------");
            }
        }
    }
}