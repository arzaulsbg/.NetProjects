namespace MovieTicketBooking.Exceptions
{
    public class InvalidLanguageException : Exception
    {
        public InvalidLanguageException(string message)
            : base(message)
        {
        }
    }
}