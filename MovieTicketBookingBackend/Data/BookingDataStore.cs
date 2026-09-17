using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class BookingDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public BookingDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddBooking(Booking booking)
        {
            Booking? existingBooking =
                GetBookingById(booking.BookingID);

            if (existingBooking != null)
            {
                return false;
            }

            context.Bookings.Add(booking);
            context.SaveChanges();

            return true;
        }

        // READ - All bookings
        public List<Booking> GetAllBookings()
        {
            return context.Bookings.ToList();
        }

        // READ - Booking by ID
        public Booking? GetBookingById(int bookingID)
        {
            return context.Bookings
                .FirstOrDefault(b => b.BookingID == bookingID);
        }

        // READ - Bookings by customer
        public List<Booking> GetBookingsByCustomer(int customerID)
        {
            return context.Bookings
                .Where(b => b.CustomerID == customerID)
                .ToList();
        }
        // READ - Bookings by show
        public List<Booking> GetBookingsByShow(int showID)
        {
            return context.Bookings
                .Where(b => b.ShowID == showID)
                .ToList();
        }
        // DELETE
        public bool DeleteBooking(int bookingID)
        {
            Booking? booking =
                GetBookingById(bookingID);

            if (booking == null)
            {
                return false;
            }

            context.Bookings.Remove(booking);
            context.SaveChanges();

            return true;
        }

        // DISPLAY
        public void DisplayAllBookings()
        {
            List<Booking> bookings =
                GetAllBookings();

            foreach (Booking booking in bookings)
            {
                booking.DisplayBookingDetails();
                Console.WriteLine("------------------------");
            }
        }
    }
}