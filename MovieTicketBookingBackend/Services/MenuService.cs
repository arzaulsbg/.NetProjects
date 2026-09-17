using MovieTicketBooking.Data;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public class MenuService
    {
        private readonly BookingService bookingService;
        private readonly MovieDataStore movieDataStore;
        private readonly TheatreDataStore theatreDataStore;
        private readonly CustomerDataStore customerDataStore;
        private readonly ShowDataStore showDataStore;
        private readonly BookingDataStore bookingDataStore;

        public MenuService(
    MovieDataStore movieDataStore,
    TheatreDataStore theatreDataStore,
    CustomerDataStore customerDataStore,
    ShowDataStore showDataStore,
    BookingDataStore bookingDataStore,
    BookingService bookingService)
        {
            this.movieDataStore = movieDataStore;
            this.theatreDataStore = theatreDataStore;
            this.customerDataStore = customerDataStore;
            this.showDataStore = showDataStore;
            this.bookingDataStore = bookingDataStore;
            this.bookingService = bookingService;
        }

        public void ShowCustomerMenu(int customerID)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=================================");
                Console.WriteLine("          CUSTOMER MENU");
                Console.WriteLine("=================================");
                Console.WriteLine("1. View All Movies");
                Console.WriteLine("2. Search Movie");
                Console.WriteLine("3. View Shows");
                Console.WriteLine("4. Book Tickets");
                Console.WriteLine("5. My Bookings");
                Console.WriteLine("6. Logout");

                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        movieDataStore.DisplayAllMovies();
                        break;

                    case "2":
                        SearchMovie();
                        break;

                    case "3":
                        showDataStore.DisplayAllShows();
                        break;

                    case "4":
                        BookTickets(customerID);
                        break;

                    case "5":
                        DisplayMyBookings(customerID);
                        break;

                    case "6":
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void BookTickets(int customerID)
        {
            Console.WriteLine();
            Console.WriteLine("========== AVAILABLE SHOWS ==========");

            showDataStore.DisplayAllShows();

            Console.Write("Enter Show ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int showID))
            {
                Console.WriteLine("Invalid Show ID.");
                return;
            }

            Show? show =
             showDataStore.GetShowById(showID);

            if (show == null)
            {
                Console.WriteLine("Show not found.");
                return;
            }

            Movie? movie =
                movieDataStore.GetMovieById(show.MovieID);

            Console.WriteLine();
            Console.WriteLine("========== SEAT RATES ==========");

            Console.WriteLine(
                $"Platinum: ₹{show.PlatinumSeatRate}"
            );

            Console.WriteLine(
                $"Gold:     ₹{show.GoldSeatRate}"
            );

            Console.WriteLine(
                $"Silver:   ₹{show.SilverSeatRate}"
            );

            Console.WriteLine();

            Console.Write("Enter Platinum seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int platinumSeats))
            {
                Console.WriteLine("Invalid seat count.");
                return;
            }

            Console.Write("Enter Gold seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int goldSeats))
            {
                Console.WriteLine("Invalid seat count.");
                return;
            }

            Console.Write("Enter Silver seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int silverSeats))
            {
                Console.WriteLine("Invalid seat count.");
                return;
            }

            if (platinumSeats < 0 ||
                goldSeats < 0 ||
                silverSeats < 0)
            {
                Console.WriteLine(
                    "Seat count cannot be negative."
                );

                return;
            }

            Booking? booking =
                bookingService.CreateBooking(
                    customerID,
                    showID,
                    platinumSeats,
                    goldSeats,
                    silverSeats
                );

            if (booking == null)
            {
                return;
            }

            // Get customer information
            Customer? customer =
                customerDataStore.GetCustomerById(customerID);

            // Get theatre information
            Theatre? theatre =
                theatreDataStore.GetTheatreById(show.TheatreID);

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("          BOOKING CONFIRMED");
            Console.WriteLine("========================================");

            Console.WriteLine();
            Console.WriteLine($"Booking ID : {booking.BookingID}");

            if (customer != null)
            {
                Console.WriteLine(
                    $"Customer   : {customer.CustomerName}"
                );
            }

            Console.WriteLine(
     $"Movie      : {movie?.MovieName ?? show.MovieID}"
 );

            if (theatre != null)
            {
                Console.WriteLine(
                    $"Theatre    : {theatre.TheatreName}"
                );
            }

            Console.WriteLine(
                $"Show Time  : {show.StartDate:dd-MM-yyyy HH:mm:ss}"
            );

            Console.WriteLine();
            Console.WriteLine("----------------------------------------");

            decimal platinumAmount =
    booking.PlatinumSeats *
    show.PlatinumSeatRate;

            decimal goldAmount =
                booking.GoldSeats *
                show.GoldSeatRate;

            decimal silverAmount =
                booking.SilverSeats *
                show.SilverSeatRate;
            Console.WriteLine(
    $"Platinum   : {booking.PlatinumSeats} × ₹{show.PlatinumSeatRate:0.##} = ₹{platinumAmount:0.##}"
);

            Console.WriteLine(
                $"Gold       : {booking.GoldSeats} × ₹{show.GoldSeatRate:0.##} = ₹{goldAmount:0.##}"
            );

            Console.WriteLine(
                $"Silver     : {booking.SilverSeats} × ₹{show.SilverSeatRate:0.##} = ₹{silverAmount:0.##}"
            );

            Console.WriteLine("----------------------------------------");

            Console.WriteLine(
                $"Total Amount: ₹{booking.TotalAmount:0.##}"
            );

            Console.WriteLine("========================================");
        }
        private void DisplayMyBookings(int customerID)
        {
            Console.WriteLine();
            Console.WriteLine("========== MY BOOKINGS ==========");

            List<Booking> bookings =
                bookingDataStore.GetBookingsByCustomer(
                    customerID
                );

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (Booking booking in bookings)
            {
                booking.DisplayBookingDetails();
                Console.WriteLine("------------------------");
            }
        }
        private void SearchMovie()
        {
            Console.Write("Enter Movie ID: ");

            string movieID =
                Console.ReadLine() ?? "";

            Movie? movie =
                movieDataStore.GetMovieById(movieID);

            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.WriteLine();
            movie.DisplayMovieDetails();
        }
        public void ShowAdminMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=================================");
                Console.WriteLine("          ADMIN MENU");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Add Movie");
                Console.WriteLine("2. View All Movies");
                Console.WriteLine("3. Search Movie");
                Console.WriteLine("4. Update Movie");
                Console.WriteLine("5. Delete Movie");
                Console.WriteLine("6. Add Theatre");
                Console.WriteLine("7. View Theatres");
                Console.WriteLine("8. Update Theatre");
                Console.WriteLine("9. Delete Theatre");
                Console.WriteLine("10. Add Show");
                Console.WriteLine("11. View Shows");
                Console.WriteLine("12. Update Show");
                Console.WriteLine("13. Delete Show");
                Console.WriteLine("14. View Bookings");
                Console.WriteLine("15. Logout");

                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddMovie();
                        break;

                    case "2":
                        movieDataStore.DisplayAllMovies();
                        break;

                    case "3":
                        SearchMovie();
                        break;

                    case "4":
                        UpdateMovie();
                        break;

                    case "5":
                        DeleteMovie();
                        break;

                    case "6":
                        AddTheatre();
                        break;

                    case "7":
                        theatreDataStore.DisplayAllTheatres();
                        break;

                    case "8":
                        UpdateTheatre();
                        break;

                    case "9":
                        DeleteTheatre();
                        break;

                    case "10":
                        AddShow();
                        break;

                    case "11":
                        showDataStore.DisplayAllShows();
                        break;

                    case "12":
                        UpdateShow();
                        break;

                    case "13":
                        DeleteShow();
                        break;

                    case "14":
                        bookingDataStore.DisplayAllBookings();
                        break;

                    case "15":
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        private void AddShow()
        {
            Console.WriteLine();
            Console.WriteLine("========== AVAILABLE MOVIES ==========");
            movieDataStore.DisplayAllMovies();

            Console.Write("Enter Movie ID: ");
            string movieID = Console.ReadLine() ?? "";

            Movie? movie =
                movieDataStore.GetMovieById(movieID);

            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("========== AVAILABLE THEATRES ==========");
            theatreDataStore.DisplayAllTheatres();

            Console.Write("Enter Theatre ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int theatreID))
            {
                Console.WriteLine("Invalid Theatre ID.");
                return;
            }

            Theatre? theatre =
                theatreDataStore.GetTheatreById(theatreID);

            if (theatre == null)
            {
                Console.WriteLine("Theatre not found.");
                return;
            }

            Console.Write("Enter start date/time (dd-MM-yyyy HH:mm:ss): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime startDate))
            {
                Console.WriteLine("Invalid date/time.");
                return;
            }

            Console.Write("Enter end date/time (dd-MM-yyyy HH:mm:ss): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime endDate))
            {
                Console.WriteLine("Invalid date/time.");
                return;
            }

            Console.Write("Enter Platinum seat rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal platinumRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            Console.Write("Enter Gold seat rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal goldRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            Console.Write("Enter Silver seat rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal silverRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            Show show = new Show(
                movieID,
                theatreID,
                startDate,
                endDate,
                platinumRate,
                goldRate,
                silverRate
            );

            bool added =
                showDataStore.AddShow(show);

            if (added)
            {
                Console.WriteLine();
                Console.WriteLine("Show added successfully.");
                Console.WriteLine($"Show ID: {show.ShowID}");
            }
            else
            {
                Console.WriteLine("Show already exists.");
            }
        }

        private void UpdateShow()
        {
            Console.Write("Enter Show ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int showID))
            {
                Console.WriteLine("Invalid Show ID.");
                return;
            }

            Show? show =
                showDataStore.GetShowById(showID);

            if (show == null)
            {
                Console.WriteLine("Show not found.");
                return;
            }

            Console.Write("Enter new start date/time (dd-MM-yyyy HH:mm:ss): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime startDate))
            {
                Console.WriteLine("Invalid date/time.");
                return;
            }

            Console.Write("Enter new end date/time (dd-MM-yyyy HH:mm:ss): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime endDate))
            {
                Console.WriteLine("Invalid date/time.");
                return;
            }

            Console.Write("Enter new Platinum rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal platinumRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            Console.Write("Enter new Gold rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal goldRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            Console.Write("Enter new Silver rate: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal silverRate))
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            bool updated =
                showDataStore.UpdateShow(
                    showID,
                    startDate,
                    endDate,
                    platinumRate,
                    goldRate,
                    silverRate
                );

            Console.WriteLine(
                updated
                    ? "Show updated successfully."
                    : "Show update failed."
            );
        }

        private void DeleteShow()
        {
            Console.Write("Enter Show ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int showID))
            {
                Console.WriteLine("Invalid Show ID.");
                return;
            }

            Show? show =
                showDataStore.GetShowById(showID);

            if (show == null)
            {
                Console.WriteLine("Show not found.");
                return;
            }

            Console.Write(
                $"Delete Show {showID}? (Y/N): "
            );

            string confirmation =
                Console.ReadLine() ?? "";

            if (confirmation.ToUpper() != "Y")
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            bool deleted =
                showDataStore.DeleteShow(showID);

            Console.WriteLine(
                deleted
                    ? "Show deleted successfully."
                    : "Show deletion failed."
            );
        }
        private void AddTheatre()
        {
            Console.Write("Enter theatre name: ");

            string theatreName =
                Console.ReadLine() ?? "";

            Console.Write("Enter total number of seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int numberOfSeats))
            {
                Console.WriteLine("Invalid number of seats.");
                return;
            }

            Console.Write("Enter Platinum seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int platinumSeats))
            {
                Console.WriteLine("Invalid Platinum seat count.");
                return;
            }

            Console.Write("Enter Gold seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int goldSeats))
            {
                Console.WriteLine("Invalid Gold seat count.");
                return;
            }

            Console.Write("Enter Silver seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int silverSeats))
            {
                Console.WriteLine("Invalid Silver seat count.");
                return;
            }

            if (platinumSeats < 0 ||
                goldSeats < 0 ||
                silverSeats < 0)
            {
                Console.WriteLine(
                    "Seat counts cannot be negative."
                );

                return;
            }

            int categoryTotal =
                platinumSeats +
                goldSeats +
                silverSeats;

            if (categoryTotal != numberOfSeats)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Seat category total must equal total seats."
                );

                Console.WriteLine(
                    $"Total seats: {numberOfSeats}"
                );

                Console.WriteLine(
                    $"Category seats: {categoryTotal}"
                );

                return;
            }

            Theatre theatre = new Theatre(
                theatreName,
                numberOfSeats,
                platinumSeats,
                goldSeats,
                silverSeats
            );

            bool added =
                theatreDataStore.AddTheatre(theatre);

            if (added)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Theatre added successfully."
                );

                Console.WriteLine(
                    $"Theatre ID: {theatre.TheatreID}"
                );
            }
            else
            {
                Console.WriteLine(
                    "Theatre already exists."
                );
            }
        }
        private void UpdateTheatre()
        {
            Console.Write("Enter Theatre ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int theatreID))
            {
                Console.WriteLine("Invalid Theatre ID.");
                return;
            }

            Theatre? theatre =
                theatreDataStore.GetTheatreById(
                    theatreID
                );

            if (theatre == null)
            {
                Console.WriteLine("Theatre not found.");
                return;
            }

            Console.Write("Enter new theatre name: ");

            string newName =
                Console.ReadLine() ?? "";

            Console.Write("Enter new number of seats: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int newSeats))
            {
                Console.WriteLine("Invalid number of seats.");
                return;
            }

            bool updated =
                theatreDataStore.UpdateTheatre(
                    theatreID,
                    newName,
                    newSeats
                );

            if (updated)
            {
                Console.WriteLine(
                    "Theatre updated successfully."
                );
            }
        }
        private void DeleteTheatre()
        {
            Console.Write("Enter Theatre ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int theatreID))
            {
                Console.WriteLine("Invalid Theatre ID.");
                return;
            }

            Theatre? theatre =
                theatreDataStore.GetTheatreById(
                    theatreID
                );

            if (theatre == null)
            {
                Console.WriteLine("Theatre not found.");
                return;
            }

            Console.Write(
                $"Delete '{theatre.TheatreName}'? (Y/N): "
            );

            string confirmation =
                Console.ReadLine() ?? "";

            if (confirmation.ToUpper() != "Y")
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            bool deleted =
                theatreDataStore.DeleteTheatre(
                    theatreID
                );

            if (deleted)
            {
                Console.WriteLine(
                    "Theatre deleted successfully."
                );
            }
        }
        private void AddMovie()
        {
            Console.Write("Enter movie name: ");
            string movieName = Console.ReadLine() ?? "";

            Console.Write("Enter director name: ");
            string directorName = Console.ReadLine() ?? "";

            Console.Write("Enter producer name: ");
            string producerName = Console.ReadLine() ?? "";

            Console.Write("Enter duration: ");

            if (!double.TryParse(
                Console.ReadLine(),
                out double duration))
            {
                Console.WriteLine("Invalid duration.");
                return;
            }

            Console.Write("Enter story: ");
            string story = Console.ReadLine() ?? "";

            Console.Write("Enter genre: ");
            string genre = Console.ReadLine() ?? "";

            Console.Write("Enter language: ");
            string language = Console.ReadLine() ?? "";

            Movie movie = new Movie(
                movieName,
                directorName,
                producerName,
                duration,
                story,
                genre,
                language
            );

            bool added = movieDataStore.AddMovie(movie);

            if (added)
            {
                Console.WriteLine();
                Console.WriteLine("Movie added successfully.");
                Console.WriteLine($"Movie ID: {movie.MovieID}");
            }
            else
            {
                Console.WriteLine("Movie already exists.");
            }
        }
        private void UpdateMovie()
        {
            Console.Write("Enter Movie ID: ");

            string movieID =
                Console.ReadLine() ?? "";

            Movie? movie =
                movieDataStore.GetMovieById(movieID);

            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.Write("Enter new movie name: ");

            string newName =
                Console.ReadLine() ?? "";

            bool updated =
                movieDataStore.UpdateMovie(
                    movieID,
                    newName
                );

            if (updated)
            {
                Console.WriteLine(
                    "Movie updated successfully."
                );
            }
        }

        private void DeleteMovie()
        {
            Console.Write("Enter Movie ID: ");

            string movieID =
                Console.ReadLine() ?? "";

            Movie? movie =
                movieDataStore.GetMovieById(movieID);

            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.Write(
                $"Delete '{movie.MovieName}'? (Y/N): "
            );

            string confirmation =
                Console.ReadLine() ?? "";

            if (confirmation.ToUpper() != "Y")
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            bool deleted =
                movieDataStore.DeleteMovie(movieID);

            if (deleted)
            {
                Console.WriteLine(
                    "Movie deleted successfully."
                );
            }
        }
    }
}