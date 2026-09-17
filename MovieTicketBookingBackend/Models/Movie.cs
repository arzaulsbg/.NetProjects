using MovieTicketBooking.Exceptions;
namespace MovieTicketBooking.Models
{
    public class Movie
    {
        public List<Show> Shows { get; set; } = new List<Show>();
        public string MovieID { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set; }
        public string ProducerName { get; set; }
        public double Duration { get; set; }
        public string Story { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public Movie()
        {
        }

        public Movie(
    string movieName,
    string directorName,
    string producerName,
    double duration,
    string story,
    string genre,
    string language)
        {
            if (duration <= 0)
            {
                throw new InvalidDurationException(
                    "Movie duration must be greater than 0."
                );
            }

            if (string.IsNullOrWhiteSpace(language))
            {
                throw new InvalidLanguageException(
                    "Movie language cannot be empty."
                );
            }

            MovieName = movieName;
            DirectorName = directorName;
            ProducerName = producerName;
            Duration = duration;
            Story = story;
            Genre = genre;
            Language = language;

            MovieID =
                    GetFirstTwoCharacters(movieName) + "-" +
                    GetFirstTwoCharacters(producerName) + "-" +
                    GetFirstTwoCharacters(genre) + "-" +
                    GetFirstTwoCharacters(language);
        }
        private string GetFirstTwoCharacters(string value)
        {
            string cleanedValue = value.Replace(" ", "");

            if (cleanedValue.Length < 2)
            {
                return cleanedValue.ToUpper();
            }

            return cleanedValue.Substring(0, 2).ToUpper();
        }
        public void DisplayMovieDetails()
        {
            Console.WriteLine($"Movie ID: {MovieID}");
            Console.WriteLine($"Movie Name: {MovieName}");
            Console.WriteLine($"Director Name: {DirectorName}");
            Console.WriteLine($"Producer Name: {ProducerName}");
            Console.WriteLine($"Duration: {Duration}");
            Console.WriteLine($"Story: {Story}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Language: {Language}");
        }
    }
}