namespace MovieTicketBooking.Exceptions
{
    public class InvalidDurationException : Exception
    {
        public InvalidDurationException(string message)
            : base(message)
        {
        }
    }
}