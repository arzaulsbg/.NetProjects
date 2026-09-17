using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class MovieDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public MovieDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddMovie(Movie movie)
        {
            Movie? existingMovie = GetMovieById(movie.MovieID);

            if (existingMovie != null)
            {
                return false;
            }

            context.Movies.Add(movie);
            context.SaveChanges();

            return true;
        }

        // READ - All movies
        public List<Movie> GetAllMovies()
        {
            return context.Movies.ToList();
        }

        // READ - Movie by ID
        public Movie? GetMovieById(string movieID)
        {
            return context.Movies
                .FirstOrDefault(m => m.MovieID == movieID);
        }

        // UPDATE
        public bool UpdateMovie(
            string movieID,
            string newMovieName)
        {
            Movie? movie = GetMovieById(movieID);

            if (movie == null)
            {
                return false;
            }

            movie.MovieName = newMovieName;

            context.SaveChanges();

            return true;
        }

        // DELETE
        public bool DeleteMovie(string movieID)
        {
            Movie? movie = GetMovieById(movieID);

            if (movie == null)
            {
                return false;
            }

            context.Movies.Remove(movie);
            context.SaveChanges();

            return true;
        }

        // DISPLAY
        public void DisplayAllMovies()
        {
            List<Movie> movies = GetAllMovies();

            foreach (Movie movie in movies)
            {
                movie.DisplayMovieDetails();
                Console.WriteLine("------------------------");
            }
        }
    }
}