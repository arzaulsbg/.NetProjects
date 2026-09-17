using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class BookingService
    {
        private MovieDataStore movieDataStore;
        private TheatreDataStore theatreDataStore;
        private ShowDataStore showDataStore;
        private CustomerDataStore customerDataStore;
        private BookingDataStore bookingDataStore;

        public BookingService(
            MovieDataStore movieDataStore,
            TheatreDataStore theatreDataStore,
            ShowDataStore showDataStore,
            CustomerDataStore customerDataStore,
            BookingDataStore bookingDataStore)
        {
            this.movieDataStore = movieDataStore;
            this.theatreDataStore = theatreDataStore;
            this.showDataStore = showDataStore;
            this.customerDataStore = customerDataStore;
            this.bookingDataStore = bookingDataStore;
        }

        // Get already booked seats for each category
        private void GetBookedSeats(
            int showID,
            out int platinumBooked,
            out int goldBooked,
            out int silverBooked)
        {
            List<Booking> bookings =
                bookingDataStore.GetBookingsByShow(showID);

            platinumBooked = 0;
            goldBooked = 0;
            silverBooked = 0;

            foreach (Booking booking in bookings)
            {
                platinumBooked += booking.PlatinumSeats;
                goldBooked += booking.GoldSeats;
                silverBooked += booking.SilverSeats;
            }
        }

        // CREATE BOOKING
        public Booking? CreateBooking(
            int customerID,
            int showID,
            int platinumSeats,
            int goldSeats,
            int silverSeats)
        {
            // Check customer
            Customer? customer =
                customerDataStore.GetCustomerById(customerID);

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return null;
            }

            // Check show
            Show? show =
                showDataStore.GetShowById(showID);

            if (show == null)
            {
                Console.WriteLine("Show not found.");
                return null;
            }

            // Check theatre
            Theatre? theatre =
                theatreDataStore.GetTheatreById(show.TheatreID);

            if (theatre == null)
            {
                Console.WriteLine("Theatre not found.");
                return null;
            }

            // Check negative values
            if (platinumSeats < 0 ||
                goldSeats < 0 ||
                silverSeats < 0)
            {
                Console.WriteLine(
                    "Seat count cannot be negative."
                );

                return null;
            }

            // Check at least one seat
            int requestedSeats =
                platinumSeats +
                goldSeats +
                silverSeats;

            if (requestedSeats <= 0)
            {
                Console.WriteLine(
                    "You must book at least one seat."
                );

                return null;
            }

            // Get already booked seats
            int platinumBooked;
            int goldBooked;
            int silverBooked;

            GetBookedSeats(
                showID,
                out platinumBooked,
                out goldBooked,
                out silverBooked
            );

            // Calculate available seats
            int platinumAvailable =
                theatre.PlatinumSeats - platinumBooked;

            int goldAvailable =
                theatre.GoldSeats - goldBooked;

            int silverAvailable =
                theatre.SilverSeats - silverBooked;

            Console.WriteLine();
            Console.WriteLine("========== AVAILABLE SEATS ==========");

            Console.WriteLine(
                $"Platinum available: {platinumAvailable}"
            );

            Console.WriteLine(
                $"Gold available:     {goldAvailable}"
            );

            Console.WriteLine(
                $"Silver available:   {silverAvailable}"
            );

            // Check Platinum
            if (platinumSeats > platinumAvailable)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Only {platinumAvailable} Platinum seats are available."
                );

                return null;
            }

            // Check Gold
            if (goldSeats > goldAvailable)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Only {goldAvailable} Gold seats are available."
                );

                return null;
            }

            // Check Silver
            if (silverSeats > silverAvailable)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Only {silverAvailable} Silver seats are available."
                );

                return null;
            }

            // Create booking
            Booking booking = new Booking(
                customerID,
                showID,
                platinumSeats,
                goldSeats,
                silverSeats,
                show.PlatinumSeatRate,
                show.GoldSeatRate,
                show.SilverSeatRate
            );

            // Save booking
            bool added =
                bookingDataStore.AddBooking(booking);

            if (!added)
            {
                Console.WriteLine(
                    "Booking could not be created."
                );

                return null;
            }

            return booking;
        }
    }
}